====== ADMIN CONTROLLER ======

resetPassword (Put) [admin auth]
url: /admin/resetpassword/{id}
input: user id
returns: Created status code, one time password in plaintext

GetUsers (Get) [admin auth]
url: /admin/users
input: none
returns: List of all users


====== LOGIN CONTROLLER ======
Ready (get)
url: /
input: none
returns: Ok status code, $"Server is ready with {userCount} user(s)."

WhoAmI (get) [auth]
url: /whoami
input: none
returns: "No token provided" or, if user has a token, User.Identity.Name

Authenticate (Post)
url: /login
input: Loginuser model (Username, password)
returns: Ok status code, ReturnUser model (id, name, role)

====== PROTEINS CONTROLLER ======
getProteins (get)
url: /proteins
input: none
returns: Ok status code, list of proteins

getProteinById (get)
url: /proteins/proteinId={id}
input: protein id
returns: Ok status code, protein by its id

getProteinsBySequenceName (get)
url: /proteins/proteinName={name}
input: sequence name
returns: Ok status code, list of proteins associated with sequence name

getProteinsByUserId (get)
url: /proteins/proteinName={Name}
input: protein name
returns: Ok status code, list of proteins associated with user id

createProtein (post)
url: /proteins
input: registerprotein model (SequenceName, ProteinSequence, Description, FormatType, UserId)
returns: 201 Created and the new protein if successful

updateProtein (put)
url: /proteins
input: proteinId to update, Protein model to send
returns: 200 Ok an the updated protein if successful

deleteProtein (delete)
url: /proteins/delete/{id}
input: protein id to delete
returns: 204 NotFound if the deletion was successful

====== LOGIN CONTROLLER ======
changePassword (put)
url: /user/changepassword
input: recoveruser model (username, password, newpassword, onetimepassword)
returns: 200 Ok if the update was sucessful

