====== ADMIN CONTROLLER ======

resetPassword (Put) [admin auth]
url: /admin/resetpassword/{id}
inputs: user id
returns: Created status code, one time password in plaintext

GetUsers (Get) [admin auth]
url: /admin/users
input: none
returns: List of all users


====== LOGIN CONTROLLER ======
Ready (get)