from fastapi import FastAPI, UploadFile, File
import cv2
import numpy as np
import os

app = FastAPI(title="Face Detection API")

MODEL_PATH = os.path.join(
    os.path.dirname(__file__),
    "models",
    "face_detection_yunet_2023mar.onnx"
)

face_detector = cv2.FaceDetectorYN.create(
    MODEL_PATH,
    "",
    (320, 320),
    0.9,
    0.3,
    5000
)


@app.get("/")
def home():
    return {"message": "Face Detection API is running"}


@app.post("/detect")
async def detect_face(file: UploadFile = File(...)):

    contents = await file.read()

    image_array = np.frombuffer(contents, np.uint8)
    image = cv2.imdecode(image_array, cv2.IMREAD_COLOR)

    if image is None:
        return {"faces": []}

    height, width = image.shape[:2]

    face_detector.setInputSize((width, height))

    _, faces = face_detector.detect(image)

    detected_faces = []

    if faces is not None:
        for face in faces:
            x, y, w, h = face[:4]

            detected_faces.append({
                "x": int(x),
                "y": int(y),
                "width": int(w),
                "height": int(h),
                "confidence": float(face[-1])
            })

    return {
        "width": width,
        "height": height,
        "faces": detected_faces
    }