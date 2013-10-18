sqrl-net
========
SQRL-net is a full SQRL implementation.  There are four basic pieces the project:

* SQRL server side library for generating server sessions and validating authentication requests.
* SQRL client side library for processing SQRL URLs and sending authentication requests.
* SQRL sample server.  An ASP.NET MVC project which uses the server library to present a login page and authenticates a user.
* SQRL sample client.  A Windows Forms app which allows the user to authenticate using the SQRL protocol.


This project is still in its early stages and is a little rough around the edges.

You can see a sample of the sample web server running at: https://sqrl.apphb.com/

### Features for the Future

* Identity management (multiple identities, import/export).
* Generate QR codes locally.
