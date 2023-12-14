USE test_capstone

BEGIN TRANSACTION;

CREATE TABLE users (
	user_id int IDENTITY(1,1) NOT NULL,
	username varchar(50) NOT NULL,
	email varchar(100) NOT NULL,
	organization_name varchar(200) NOT NULL,
	password_hash varchar(200) NOT NULL,
	salt varchar(200) NOT NULL,
	user_role varchar(50) NOT NULL,
	has_one_time_password bit NOT NULL,
	CONSTRAINT PK_user PRIMARY KEY (user_id)
)
CREATE TABLE proteins (
	protein_id int IDENTITY(1,1) NOT NULL,
	sequence_name varchar(500) NOT NULL,
	protein_sequence varchar(8000) NOT NULL,
	description varchar(8000) NOT NULL,
	format_type int NOT NULL,
	username varchar(200) NOT NULL,
	user_id int NOT NULL
	CONSTRAINT PK_protein PRIMARY KEY (protein_id)
	FOREIGN KEY (user_id) REFERENCES users(user_id)
)
CREATE TABLE homeview(
view_id int IDENTITY(1,1) NOT NULL,
name varchar(100) NOT NULL,
header varchar(500) NOT NULL,
body varchar(8000) NOT NULL, 
active bit NOT NULL,
image_source varchar(200) NOT NULL,
CONSTRAINT view_id PRIMARY KEY(view_id)
) 
--populate default data
INSERT INTO users (username, email, organization_name, password_hash, salt, user_role, has_one_time_password) VALUES ('user', 'dummy@email.net', 'Center for Charitable Contributions', 'Jg45HuwT7PZkfuKTz6IB90CtWY4=','LHxP4Xh7bN0=','user', 0);
INSERT INTO users (username, email, organization_name, password_hash, salt, user_role, has_one_time_password) VALUES ('admin','smart@email.com', 'Association For Protein Enrichment','YhyGVQ+Ch69n4JMBncM4lNF/i9s=', 'Ar/aB2thQTI=','admin', 0);

INSERT INTO homeview (header, body, active, image_source, name) VALUES ('Protein Capture Science', 'Amino Acid Sifter', 1, 'src/img/AminoAcid.jpg', 'Default')
INSERT INTO homeview (header, body, active, image_source, name) VALUES ('HO HO HO', 'Merry Christmas', 0, 'src/img/AminoAcid.jpg', 'Christmas')
INSERT INTO homeview (header, body, active, image_source, name) VALUES ('Software is a great combination between artistry and engineering', '-Bill Gates', 0, 'src/image/Empty', 'Quote')

INSERT INTO proteins (sequence_name, protein_sequence, description, format_type, username, user_id) 
VALUES ('Insulin', 'MALWMRLLPLLALLALWGPDPAAAFVNQHLCGSHLVEALYLVCGERGFFYTPKTRREAEDLQASALSLSSSTSTWPEGLDATARAPPALVVTANIGQAGGSSSRQFRQRALGTSDSPVLFIHCPGAAGTAQGLEYRGRRVTTELVWEEVDSSPQPQGSESLPAQPPAQPAPQPEPQQAREPSPEVSCCGLWPRRPQRSQN', 'This is an insulin protein!', 1, 'user', 1);


COMMIT;