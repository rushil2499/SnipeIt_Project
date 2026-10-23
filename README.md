# Snipe-IT Test Automation Framework (.NET & Playwright)

An end-to-end automation test suite built with **C#**, **.NET**, **Microsoft Playwright**, and **NUnit** covering the lifecycle of asset creation and audit verification on Snipe-IT.

---

## Architectural Highlights & Best Practices

- **Page Object Model (POM):** Encapsulates page locators, asynchronous Select2 dropdown handlers, and AJAX-backed table interactions away from test logic.
- **Dynamic Test Data:** Auto-generates unique, timestamped asset tags to avoid collisions in shared demo environments.
- **Resilient Synchronization:** Implements Playwright web-first assertions and dynamic wait states rather than arbitrary sleeps, gracefully handling asynchronous Bootstrap Table renders.
- **Observability & Diagnostics:** Includes automated full-page screenshot capture and comprehensive Playwright Tracing (`.zip` containing network calls, console logs, and DOM snapshots) on test failures.

---

## Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download) (.NET 8.0, 9.0, or 10.0)
- [PowerShell](https://github.com/PowerShell/PowerShell) (used by Playwright driver scripts)

---

## Getting Started & Execution

### 1. Clone the Repository
```bash
git clone <YOUR_REPO_URL>
cd SnipeIt_Project