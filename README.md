# CyberSecurityBase Project1 - Feedback App

## About CyberSecurityBase course

This is a excersice project related to Cyber Security Base 2019-2020 course (https://cybersecuritybase.mooc.fi/.

## Project description

This sample application has different flaws from the OWASP top ten list (https://www.owasp.org/images/7/72/OWASP_Top_10-2017_%28en%29.pdf.pdf). Flaws are descripted later on this document.

## About project technologies

- CyberSecurityBase.Feedback.Api: App's API back-end is created with .NET Core 2.1.
- CyberSecurityBase.Feedback.Web: App's front-end is a SPA application created with Angular 5.
- CyberSecurityBase.Feedback.Idp: Authentication to feedback App is handled with OpenId Connect/OAuth powered by Identity Server 4.

## Installation instructions and how to start

- Install Visual Studio 2019 (ex. Community edition) - https://visualstudio.microsoft.com/downloads/.
- Install NodeJs - https://nodejs.org/en/.
- Rebuild the solution and all packaged should be restored automatically.
- Press F5 and all applications should be started from IIS Express webserver.
- First open Feedback UI application (CyberSecurityBase.Feedback.Web) which can be found from https://localhost:44376/.

### Test credentials

- Normal user - uid: alice pwd: alice
- Admin user - uid: bob pwd: bob

## OWASP TOP 10 flaws in the application

### FLAW 1: A1:2017-Injection (SQL Injection)
### FLAW 2: A2:2017-Broken Authentication
### FLAW 3: A5:2017-Broken Access Control
### FLAW 4: A6:2017-Security Misconfiguration
### FLAW 5: A7:2017-Cross-Site Scripting (XSS)
### FLAW 6: A9:2017-Using Components with Known Vulnerabilities
### FLAW 7: A10:2017-Insufficient Logging&Monitoring
