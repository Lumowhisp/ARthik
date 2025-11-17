

# 🪙 ARthik — AR-Based Currency Converter  

**ARthik** is an Augmented Reality (AR) powered currency converter app that lets users scan real-world currency notes and instantly view their value in other currencies using AR overlays.  
It combines **Vuforia Engine**, **Unity**, and **real-time exchange APIs** to make financial learning and conversion fun, visual, and interactive.

---

## 🚀 Features
- 🔍 **AR Currency Scanning:** Detect and recognize currency notes using Vuforia Image Targets.  
- 💱 **Live Conversion:** Convert scanned currency to multiple currencies in real-time.  
- 📊 **SmartBillC:** View detailed bill breakdowns using AI explanations.  
- 🧠 **Offline Mode:** Works seamlessly even without an internet connection.  
- 📍 **Location-Based Currency:** Auto-detects your region and sets default currency.  
- ⚡ **Lightweight & Fast:** Optimized for mobile AR performance.  

---

## 🧩 Tech Stack
- **Engine:** Unity (2022+)  
- **AR SDK:** Vuforia Engine  
- **Backend:** Firebase  
- **Frontend/UI:** Unity UI Toolkit  
- **Languages:** C#  
- **Platforms:** Android (APK Builds)  

---

## 📂 Project Structure
```
ARthik/
 ┣ Assets/
 ┃ ┣ Scenes/              # Unity scenes (ARthik, Card, WelcomeUI)
 ┃ ┣ Scripts/             # All C# scripts
 ┃ ┣ Images/              # UI images & icons
 ┃ ┗ Prefabs/             # Reusable prefabs
 ┣ Packages/
 ┣ ProjectSettings/
 ┗ UserSettings/
```

---

## ⚙️ Setup Instructions
1. Clone the repository:
   ```bash
   git clone https://github.com/<your-username>/ARthik.git
   ```
2. Open the project in **Unity Hub** (recommended Unity 2022.3+).  
3. Add the **Vuforia Engine** package (if not already imported).  
4. Set up your **Vuforia License Key** in `ARCamera`.  
5. Build the project for Android:  
   `File → Build Settings → Android → Build`.  

---

## 📸 Scenes Overview
| Scene Name | Description |
|-------------|-------------|
| **WelcomeUI** | The app’s home screen with navigation buttons. |
| **Card** | AR scene for scanning currency cards. |
| **ARthik** | Main AR currency converter scene. |

---

## 🧾 Version History
| Version | Status | Notes |
|----------|---------|------|
| 1.00–1.09 | Beta | Initial builds and UI tests |
| 2.01–2.07 | Testing | Added ARthik UI, improved performance|
| 2.08–2.09 | Stable | Added ARthik UI, improved performance,DataBase Updated,Last Update Feature for API|

---

## 📜 License
This project is currently **proprietary** under the Creator **Aditya (Founder of ARthik)**.  
All rights reserved © 2025.