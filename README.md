# CyberSecurityBase Project1 - Feedback App

## About CyberSecurityBase course

This is a exercise project related to Cyber Security Base 2019-2020 course (https://cybersecuritybase.mooc.fi/).

## Project description

This sample application has different flaws from the OWASP top ten list (https://www.owasp.org/images/7/72/OWASP_Top_10-2017_%28en%29.pdf.pdf). Flaws are descripted later on this document.

Sample application is a Feedback system which has an API endpoint and UI application. User has to login to Feedback UI via IDP service before submitting new feedback. UI application has a dependency on the API and API to the database.

### Test credentials

- Normal user - uid: alice pwd: alice
- Admin user - uid: bob pwd: bob

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

## OWASP TOP 10 flaws in the application

### FLAW 1: A1:2017-Injection (SQL Injection)

API endpoint has methods for creating and fetching feedback. Queries to the database are handled via Entity Framework library which is widely used technology in the .NET applications. Basically Entity Framework is a wrapper library which creates SQL-queries automatically behind the scenes. Database queries are created with the LINQ expressions which basically enables that application developer does not need to know SQL syntax. Of course it’s prerequisite that developer really knows the SQL syntax. Sometimes queries created by Entity Framework are not the most optimal.

LINQ based queries are protected from SQL injections by default which is a great benefit. But like said sometimes there are cases where it’s more optimal to create SQL sentences manually and execute it via Entity Framework engine. However this approach has great possibility to SQL injection vulnerability especially if parameterized queries are not used.

In the example application Feedback API has search method which uses raw SQL wildcard (like operator) query to fetch feedbacks from the database.

```
var query = $"SELECT * FROM Feedbacks WHERE UserId = '{sub}' AND Content LIKE '%{keywords}%'";
var results = _context.Feedbacks.FromSql(query).ToList();
```
This approach enables that normal end user could inject ex. like this %' OR '%'=' to the search box. After that the following SQL query is executed and it returns all feedback from all users:

```
SELECT * FROM Feedbacks WHERE UserId = '4' AND Content LIKE '%%' OR '%'='%'
```
This vulnerability can be fixed to change query to use LINQ expressions or raw SQL with SQL parameters.

SQL injection protected query which uses SQL parameters:

```
var keywordParameter = new SqliteParameter("@Keyword", "%" + keywords + "%");
var subParameter = new SqliteParameter("@Sub", sub);
var results = _context.Feedbacks.FromSql($"SELECT * FROM Feedbacks WHERE UserId = @Sub AND Content LIKE @Keyword", subParameter, keywordParameter).ToList();
```

### FLAW 2: A2:2017-Broken Authentication

Due to XSS vulnerability of the system user's access/session tokens are able to steal with an injected script.

Fix of the XSS vulnerability is described in the section Flaw 7.

### FLAW 3: A5:2017-Broken Access Control

Feedback system has two types of users, admins and normal users. Back-end API has authorization in all methods but the role of the logged in user is not verified. Implemented authorization just verifies that user is authenticated nothing else. Current API application allows a normal user to search all other user's feedback if user just knows the URL of the search API method (GET https://localhost:44330/api/feedback/admin + bearer token in authorization header).

Role of the user has to verified in the back-end side. Authorize attribute supports the usage of policies. Role based policy should be configured to the startup configuration of the API application. Admin policy should be then added to the search API method.

### FLAW 4: A6:2017-Security Misconfiguration

Feedback API has security misconfiguration related to CORS (Cross-Origin Resource Sharing). API allows access from all origins with all verbs and headers. Basically this vulnerability allows usage of the API services from all other external applications via javascript.

CORS policy of the API application should be hardened to fix the problem. In this scenario only allowed origin must be the URL address of the Feedback UI application. Method level CORS hardening should be also considered.

```
services.AddCors(options =>
{
  options.AddPolicy(“FeedbackUI”,
    builder =>
    {
      builder.WithOrigins("https://localhost:44376");
    });
});
```

### FLAW 5: A7:2017-Cross-Site Scripting (XSS)

Feedback input form has WYSIWYG editor and it allows adding rich content but all inputted content is encoded by the default so scripting is not possible. Feedback listing component shows all feedback from all users and it allows to render rich HTML content (full trust) because it’s explicitly enabled via Angular safe pipe. Application developer has made a mistake because there is no reason to allow explicitly html full trust.

```
<td><p [innerHTML]="feedback.content | safe: 'html'"></p></td>
```

Even though it’s not directly possible to inject script via UI, a hacker could use external tools to bypass WYSIWYG editor content encoding. Feedback content is not XSS-verified in the back-end API so it’s possible to use tools like Postman to execute API request to create a new feedback. API requires that user is authenticated so a hacker could create a fake account and get the access token which can be added manually to the API request.

With Postman hacker could execute the request with the following data (parameters) which contains injected script:

```
{"firstName":"Alice","lastName":"Smith","subject":"","email":"AliceSmith@email.com","content":"<img src=\"invalid_link\" onerror=\"alert('ok');\">"}
```

Next time when admin user opens the feedback listing view the injected script will be executed. Injected script could redirect admin user to the phishing site or steal tokens etc.

Vulnerability should be fixed so that safe pipe functionality should be removed which allowed full trust HTML content.

```
<td><p [innerHTML]="feedback.content"></p></td>
```

### FLAW 6: A9:2017-Using Components with Known Vulnerabilities

Feedback UI application has multiple old external libraries used. NPM audit command reveals that there is a 234 vulnerabilities (21 low, 80 moderate, 132 high, 1 critical) in 7175 scanned packages of the application.

These external packages should be updated to the newest version. Lifecycle of the external packages should be monitored all the time.

### FLAW 7: A10:2017-Insufficient Logging&Monitoring

Back-end API application has no monitoring or logging. Hacker could repeat attacks multiple times without log event creation.

Logging and monitoring features should be added to the API. It’s important that suspicious and repeating events are logged.
