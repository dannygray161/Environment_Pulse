# Environment_Pulse (v1.0 Alpha)

**Environment_Pulse** is an experimental, public-facing diagnostic tool developed to monitor local system health and ISP stability. This utility provides real-time visual confirmation of environment readiness and identifies intermittent internet connectivity drops, specifically optimized for **Spectrum outage** detection.

---

## Primary Objectives
* **Outage Detection:** Identifies "silent" internet drops by polling high-availability external gateways.
* **Latency Benchmarking:** Establishes baseline round-trip times to identify ISP throttling or local network congestion.
* **Resource Awareness:** Monitors local storage to ensure the host system remains performant for logging and operations.

---

## Features

### 1. ISP Connectivity & Latency Tracking
Utilizes the `Ping` class to verify gateway reachability.
* **Target:** `8.8.8.8` (Google Public DNS).
* **Timeout:** 3000ms to prevent application hangs during outages.
* **Metrics:** Captures `IPStatus` and `RoundtripTime` (ms) into global state.



### 2. Storage Health Assessment
Monitors the primary system partition using `DriveInfo`.
* **Target:** `C:` Drive.
* **Threshold:** Triggers a "Low Storage" state at $\le$ 30% free space.
* **Formatting:** Displays data rounded to two decimal places (`F2`) for high readability.

### 3. Correlated System Pulse
Aggregates global states to provide a prioritized status report:
* **CRITICAL:** Both connectivity and storage are below acceptable thresholds.
* **OFFLINE:** Connectivity is lost (Status != Success), regardless of storage.
* **STORAGE LOW:** Internet is active, but local storage is $\le$ 30%.
* **PASS:** All systems within optimal operating parameters.

---

## Technical Specifications

| Component | Logic | Configuration |
| :--- | :--- | :--- |
| **Runtime** | .NET | Console Application |
| **Network API** | `System.Net.NetworkInformation` | ICMP / IPv4 |
| **Storage API** | `System.IO.DriveInfo` | Fixed Disk Analytics |
| **Error Handling** | `Try-Catch-Finally` | Defensive state resets to `Unknown` |
| **State Management** | Global Static Fields | Persisted across method calls |

---

## Usage Instructions

1. **Launch:** Enter the main command loop via the CLI.
2. **Select Option:**
    * `1`: Manual Disk Health Scan.
    * `2`: Manual Connectivity & Latency Test.
    * `3`: Correlated System Pulse (Aggregated Report).
    * `4`: Terminate Program.
3. **Visual Feedback:**
    * **Green/Black:** System Healthy.
    * **Yellow/Black:** Connectivity Issues detected.
    * **Red/White:** Critical Resource Failure.
    * **Magenta/White:** Specific Storage or Configuration Warnings.



---

## Known Issues & Limitations
* **Hardcoded Targets:** Currently hardcoded to `8.8.8.8` and `C:`.
* **Admin Privileges:** Some systems may require elevated permissions for full hardware access.

---

## Author
**dannygray161** *Created: February 26, 2026*