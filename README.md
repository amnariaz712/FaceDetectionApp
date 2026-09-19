# Face Detection App

A real-time face detection web application that captures camera frames, sends them to a Python API, and draws a green bounding box around detected faces in the browser.

Built as an internship project combining an **ASP.NET Core MVC** frontend with a **Python FastAPI** backend powered by **OpenCV** and the **YuNet** face detection model.

---

## How It Works

```
Camera
   ↓
ASP.NET Core Web Application
   ↓
Python FastAPI
   ↓
OpenCV + YuNet
   ↓
Face Detection
   ↓
Detection Result
   ↓
Green Bounding Box
```

The camera captures frames, the ASP.NET app forwards them to the Python API, the API runs YuNet inference through OpenCV, and the detected face coordinates are returned and drawn as a green bounding box on the client.

---

## Features

- Real-time face detection from a webcam feed
- Automatic camera capture in the browser
- Green bounding box drawn around each detected face
- REST API for face detection
- Automatic Python API startup after reboot (Task Scheduler)
- IIS-hosted ASP.NET Core frontend

---

## Technologies Used

**Backend / Face Detection**
- Python
- FastAPI
- Uvicorn
- OpenCV
- YuNet face detection model (`face_detection_yunet_2023mar.onnx`)
- ONNX

**Web Application**
- ASP.NET Core MVC
- C#
- HTML / CSS / JavaScript

**Deployment**
- IIS
- Windows Task Scheduler
- Windows Server

---

## Project Structure

```text
FaceDetectionApp
│
├── PythonAPI
│   ├── models
│   │   └── face_detection_yunet_2023mar.onnx
│   ├── app.py
│   ├── start_api.bat
│   └── requirements.txt
│
└── WebApp
    └── FaceDetectionWeb
        ├── FaceDetectionWeb.sln
        └── FaceDetectionWeb
            ├── Controllers
            ├── Models
            ├── Properties
            ├── Views
            ├── wwwroot
            ├── appsettings.json
            ├── appsettings.Development.json
            ├── FaceDetectionWeb.csproj
            └── Program.cs
```

> The `Published/`, `venv/`, `__pycache__/`, `.vs/`, `bin/` and `obj/` folders are generated locally and are intentionally excluded from Git.

---

## Python API

The Python backend runs using **FastAPI** and **Uvicorn**.

During development the API is hosted at:

```
http://127.0.0.1:8000
```

It uses the **YuNet** model stored in:

```
PythonAPI/models/face_detection_yunet_2023mar.onnx
```

### Running the Python API

```bash
cd PythonAPI

# Create and activate a virtual environment (first time only)
python -m venv venv
venv\Scripts\activate

# Install dependencies
pip install -r requirements.txt

# Start the API
uvicorn app:app --host 127.0.0.1 --port 8000
```

API will be available at `http://127.0.0.1:8000`.
Interactive docs (Swagger UI) at `http://127.0.0.1:8000/docs`.

---

## ASP.NET Core Web Application

The frontend is an **ASP.NET Core MVC** application.

Solution file:

```
WebApp/FaceDetectionWeb/FaceDetectionWeb.sln
```

Main project:

```
WebApp/FaceDetectionWeb/FaceDetectionWeb/
```

### Running the Web Application

1. Open `FaceDetectionWeb.sln` in **Visual Studio**.
2. Restore NuGet packages.
3. Build the solution.
4. Run the ASP.NET Core project (F5 / Ctrl+F5).
5. Make sure the Python API is already running on `http://127.0.0.1:8000`.

---

## Deployment Architecture

```
                    Windows Server
                          │
             ┌────────────┴────────────┐
             │                         │
             ▼                         ▼
           IIS                  Task Scheduler
             │                         │
             ▼                         ▼
    ASP.NET Core MVC             start_api.bat
       Port 8080                      │
                                      ▼
                              FastAPI + Uvicorn
                                  Port 8000
                                      │
                                      ▼
                              OpenCV + YuNet
                                      │
                                      ▼
                              Face Detection
```

### IIS Configuration

- ASP.NET Core app published to: `D:\FaceDetectionApp\Published`
- Hosted through **IIS** on port **8080**
- Accessible at `http://localhost:8080` (or via server IP on the internal network)

### Automatic Python API Startup

`PythonAPI/start_api.bat` launches the FastAPI server.

**Windows Task Scheduler** is configured to run this batch file automatically after reboot, so the API recovers without manual intervention.

---

## Important Configuration

Before deployment, ensure the Python API URL in the ASP.NET app matches the actual API address.

**Development:**

```
http://127.0.0.1:8000
```

If the API is deployed on a different machine, update the URL in the ASP.NET configuration (e.g. `appsettings.json`) to the appropriate server IP.

---

## What I Learned

- Building a web application with ASP.NET Core MVC
- Connecting an ASP.NET application to a Python API
- Creating REST APIs with FastAPI
- Running APIs with Uvicorn
- Using OpenCV for computer vision
- Working with a pre-trained YuNet face detection model
- Integrating Python and .NET applications
- Publishing ASP.NET Core applications
- Hosting applications through IIS
- Configuring Windows Task Scheduler
- Troubleshooting application deployment on Windows
- Managing Python virtual environments and dependencies

---

## Author

**Amna Riaz**  
GitHub: [@amnariaz712](https://github.com/amnariaz712)

---

## License

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.