# 🏥 CityCare Clinic Management System

A full-stack web application built to streamline clinic operations — role-based access for Administrators, Doctors, and Patients, with appointment scheduling, symptom-based doctor recommendations, and system-wide activity logging.

![Status](https://img.shields.io/badge/status-active-brightgreen)
![.NET](https://img.shields.io/badge/backend-ASP.NET%20Core-512BD4)
![Angular](https://img.shields.io/badge/frontend-Angular-DD0031)
![License](https://img.shields.io/badge/license-MIT-blue)

---

## 📋 Table of Contents
- [Overview](#-overview)
- [Architecture](#-architecture)
- [Features](#-features)
- [OOP Principles Applied](#-oop-principles-applied)
- [Storage](#-storage)
- [Getting Started](#-getting-started)
- [Contributing](#-contributing)
- [License](#-license)

---

## 🔍 Overview

CityCare Clinic Management System is a comprehensive, full-stack web application designed to streamline clinic operations. It provides secure, role-based access for **Administrators**, **Doctors**, and **Patients**, handling directory management, appointment scheduling, symptom-based doctor recommendations, and system-wide activity logging.

---

## 🏗 Architecture

Built on a modern **3-Tier (Client-Server) Architecture**:

| Layer | Technology | Responsibility |
|---|---|---|
| **Frontend** | Angular + SCSS | Single Page Application (SPA) consuming the backend REST API, with a custom responsive UI design system |
| **Backend (Web API)** | ASP.NET Core Web API (C#) | Routes HTTP requests via `AuthController`, `DoctorsController`, `PatientsController`, `AppointmentsController`, `HistoryController` |
| **Business Logic (BL)** | C# | Core business rules and validation, fully decoupled from the UI — `ClinicService`, `DoctorManager`, `PatientManager`, `AppointmentManager` |
| **Data Layer (DL)** | Flat-file storage (`.txt`) | Data persistence via `FileManager` and entity models (`User`, `Doctor`, `Patient`, `Appointment`) |

---

## ✨ Features

### 👨‍💼 Administrator
- Register, edit, and delete doctor records; assign specialties
- Register, edit, and remove patient records
- View a master list of all scheduled appointments clinic-wide
- Monitor a real-time, terminal-style log of all system activity (logins, bookings, deletions)

### 🩺 Doctor
- View confirmed appointments assigned to them
- Access a patient directory with contact info and reported symptoms
- Securely update their portal password

### 🤒 Patient
- Get a **smart doctor recommendation** based on symptoms (e.g., "chest pain" → Cardiologist)
- Book appointments by selecting a doctor, date, and available time slot
- View and manage existing appointments
- Self-manage account password

---

## 🧱 OOP Principles Applied

The C# backend is built around solid OOP fundamentals:

1. **Inheritance** — Abstract base class `User` (Name, Password) is extended by `Doctor` and `Patient` with role-specific properties (`Specialization`, `Symptoms`).
2. **Encapsulation** — Private fields backing public properties; manager classes (`DoctorManager`, `PatientManager`, `AppointmentManager`) encapsulate internal state and expose controlled methods like `AddDoctor` and `AuthenticateDoctor`.
3. **Polymorphism** — Abstract `DisplayInfo()` method on `User` is overridden by `Doctor` and `Patient` to render role-specific information.
4. **Abstraction** — `User` is an abstract class serving as a conceptual template; Controllers interact only with `ClinicService`, never with raw data storage.
5. **Design Patterns**
   - **Façade Pattern** — `ClinicService` provides a single simplified interface over the BL subsystems.
   - **Repository-like Pattern** — `FileManager` centralizes all file I/O, abstracting `StreamReader`/`StreamWriter` logic from the BL.

---

## 💾 Storage

| File | Purpose |
|---|---|
| `doctors.txt` | Serialized doctor records |
| `patients.txt` | Serialized patient records |
| `appointments.txt` | Booking records |
| `activity_history.txt` | Append-only system activity log |

---

## 🚀 Getting Started

```bash
# Clone the repository
git clone https://github.com/<your-username>/citycare-clinic-management.git
cd citycare-clinic-management

# Backend (ASP.NET Core)
cd Backend
dotnet restore
dotnet run

# Frontend (Angular)
cd ../Frontend
npm install
ng serve
```

> Update connection/API base URL settings as needed for your local environment.

---

## 🤝 Contributing

Contributions, issues, and feature requests are welcome. Feel free to open an issue or submit a pull request.

---

## 📄 License

This project is licensed under the MIT License — see the `LICENSE` file for details.
