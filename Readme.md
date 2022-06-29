This is a test application for DB calls on SQL Server (mainly for Android)
- there is an issue that SQL Server Express 2019 connection fails on all Android devices
- there is an issue that SQL Server 2012 (with latest patch, still supported) connection fails on devices Android 8.1 and lower

This application executes a dummy query (SELECT LEN('Hello World')) using various versions of SqlClient to test the connection.
If a known error is received, the application shows a friendly messsage (e.g. Android connection bug or check internet connection)
Theoretical result for all DB calls is "success"
The application also shows SQL Connection IDs if the connection connects succesfully to the server and the call is finished

The application has multiple options to configure the connection string (enable/disable data encryption, disable connection pooling, trust server certificate).

There are 3 predefined connections:
- all require the database devRemin database
- 1. connection to local SQL Express 2019 -> .\SQLEXPRESS tries to connect to a local instance of SQL Server Express called "SQLEXPRESS" (default name)
  - this is usable only when accessing from the same machine which is running the SQL Server Express instance
- 2. connection to local SQL Exprses 2019 (.....) with a special IP 10.0.2.2 which can be used in Android emulator to access services running on the parent device as if it were the same machine
  - this requires so that the SQL Express is configured for TCP/IP connections and SQL (login & password) authentication
- 3. connection via a private/local IP
  - this requires that the local IP matches (probably won't in a different network)
  - requires TCP/IP connections enabled on the SQL Server Express instance as well