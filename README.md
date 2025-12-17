# 📄 PaymentAdventuresLab

![.NET](https://img.shields.io/badge/.NET-8.0-blueviolet)
![ASP.NET MVC](https://img.shields.io/badge/ASP.NET%20Core-MVC-blue)
![License](https://img.shields.io/badge/license-MIT-green)
![Status](https://img.shields.io/badge/status-learning%20project-yellow)
![PDF](https://img.shields.io/badge/PDF-QuestPDF-red)

**PaymentAdventuresLab** is a focused **ASP.NET Core MVC** learning project that demonstrates **server-side invoice PDF generation**, form processing, validation, and safe file handling using **QuestPDF**.

This project intentionally avoids database persistence, authentication, and external integrations in order to **deeply explore the MVC flow and PDF generation pipeline** in a clean and understandable way.

> ⚠️ This is a **learning / pet project**, not a production billing system.

---

## 🎯 Project Goals

The main goal of this project is to practice and demonstrate:

- Server-side PDF generation with **QuestPDF**
- Proper **ASP.NET Core MVC architecture**
- Form submission and **server-side validation**
- Safe interaction with the file system
- Secure file downloads with correct MIME handling
- Graceful error handling for real-world edge cases

---

## 🧠 What This Project Demonstrates

- End-to-end MVC flow: **View → Controller → Service → File → Download**
- Server-side document generation (no client-side hacks)
- Validation of user input before file creation
- Defensive programming:
  - automatic folder creation
  - missing file handling (404)
- Separation of concerns between UI and PDF rendering logic

---

## ✨ Features

- Invoice input form (dates, amount, customer details)
- Server-side validation of form data
- Conversion of numeric amounts into **rubles / kopeks** and text representation
- PDF invoice generation using **QuestPDF**
- Automatic creation of the output directory if it does not exist
- Secure PDF download with proper content headers
- Graceful error handling when files are missing
- Locally stored generated PDFs

Generated PDFs are saved to:

```
FolderForPdf/
```

---

## 🛠️ Tech Stack

- **ASP.NET Core 8 (MVC)**
- **C#**
- **QuestPDF** — PDF document generation
- Razor Views
- Basic HTML / CSS / JavaScript

> 🎨 UI styling was assisted with AI tools.  
> 🧠 All application logic, architecture, and server-side code were implemented by the author.

---

## 🚀 Getting Started

### Prerequisites

- .NET 8 SDK

### Run Locally

```bash
git clone https://github.com/LevShapovalov/PaymentAdventuresLab.git
cd PaymentAdventuresLab
dotnet restore
dotnet run
```

The application will start on the default ASP.NET Core port, for example:

```
http://localhost:5000
```

---

## 🧭 How to Use

1. Open:
   ```
   /Home/DoPayment
   ```
2. Fill in the required invoice fields
3. Submit the form
4. The PDF invoice will be generated and saved to:
   ```
   FolderForPdf/
   ```
5. Download the generated invoice using the provided link

---

## 📂 Project Structure

```
PaymentAdventuresLab/
├── Controllers/
│   └── HomeController.cs        # Form handling, validation, PDF generation, file download
├── FolderPdfGenerator/
│   └── PdfGenerator.cs          # QuestPDF document template and rendering logic
├── FolderForPdf/                # Generated PDFs (.gitkeep included)
├── Views/
│   └── Home/
│       └── DoPayment.cshtml
├── wwwroot/
│   ├── css/
│   └── js/
├── .gitignore
└── README.md
```

---

## ⚠️ Known Limitations

- Placeholder values are used in the PDF template (e.g. `<<Company Name>>`)
- No database or invoice history storage
- Minimal validation (no localization or currency switching)
- No authentication or user accounts
- No email / messaging integrations
- `PuppeteerSharp` is referenced but not used (left from an experimental approach)

---

## 🔧 Easy Improvements (5–15 minutes)

- Add validation attributes to the invoice view model
- Improve form UX (inline validation messages, hints)
- Log PDF generation and download errors
- Replace placeholders with configurable company data
- Improve PDF layout (tables, totals, VAT, branding)

---

## 📌 Project Status

- ✔ Learning / Pet Project
- ✔ Core functionality implemented
- ✔ Stable local execution
- ❌ Not intended for production use

---

## 📜 License

MIT License.  
See the `LICENSE` file for details.

Please review **QuestPDF licensing terms** before any commercial usage.
