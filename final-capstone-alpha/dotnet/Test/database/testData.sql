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
	user_id int NOT NULL,
	sequence_1 varchar(8000),
	sequence_2 varchar(8000),
	sequence_3 varchar(8000),
	CONSTRAINT PK_protein PRIMARY KEY (protein_id),
	FOREIGN KEY (user_id) REFERENCES users(user_id)
)

CREATE TABLE homeview (
	view_id int IDENTITY(1,1) NOT NULL,
	name varchar(100) NOT NULL,
	header varchar(500) NOT NULL,
	body varchar(8000) NOT NULL, 
	active bit NOT NULL,
	image_source varchar(200) NOT NULL,
	CONSTRAINT view_id PRIMARY KEY(view_id)
) 

CREATE TABLE cells (
	cell_id int IDENTITY(1,1) NOT NULL,
	x_cord int NOT NULL,
	y_cord int NOT NULL,
	letter_x varchar(1) NOT NULL,
	letter_y varchar(1) NOT NULL,
	color varchar(10) NOT NULL,
	acid varchar(1) NOT NULL
)

--populate default data
INSERT INTO users (username, email, organization_name, password_hash, salt, user_role, has_one_time_password) VALUES ('user', 'dummy@email.net', 'Center for Charitable Contributions', 'Jg45HuwT7PZkfuKTz6IB90CtWY4=','LHxP4Xh7bN0=','user', 0);
INSERT INTO users (username, email, organization_name, password_hash, salt, user_role, has_one_time_password) VALUES ('admin','smart@email.com', 'Association For Protein Enrichment','YhyGVQ+Ch69n4JMBncM4lNF/i9s=', 'Ar/aB2thQTI=','admin', 0);
INSERT INTO users (username, email, organization_name, password_hash, salt, user_role, has_one_time_password) VALUES ('new user!', 'user@yahoo.com', 'new startup industries', 'SJwGoscVAqnsELPHPdMhq6tqYEM=', 'I7cqSJ3qfU4=', 'user', 0);
INSERT INTO users (username, email, organization_name, password_hash, salt, user_role, has_one_time_password) VALUES ('new admin', 'admin@gmail.com', 'administrative experts incorporated', '7aAwUxYHuUaz2iNz3SjHZHaZq88=', 'ydNeoqlGX9I=', 'admin', 0);


INSERT INTO homeview (header, body, active, image_source, name) VALUES ('Protein Capture Science', 'Amino Acid Sifter', 1, 'src/img/AminoAcid.jpg', 'Default')
INSERT INTO homeview (header, body, active, image_source, name) VALUES ('HO HO HO', 'Merry Christmas', 0, 'src/img/AminoAcid.jpg', 'Christmas')
INSERT INTO homeview (header, body, active, image_source, name) VALUES ('Software is a great combination between artistry and engineering', '-Bill Gates', 0, 'src/image/Empty', 'Quote')

INSERT INTO proteins (sequence_name, protein_sequence, description, format_type, username, user_id) 
VALUES ('Insulin', 'MALWMRLLPLLALLALWGPDPAAAFVNQHLCGSHLVEALYLVCGERGFFYTPKTRREAEDLQASALSLSSSTSTWPEGLDATARAPPALVVTANIGQAGGSSSRQFRQRALGTSDSPVLFIHCPGAAGTAQGLEYRGRRVTTELVWEEVDSSPQPQGSESLPAQPPAQPAPQPEPQQAREPSPEVSCCGLWPRRPQRSQN', 'This is an insulin protein!', 1, 'user', 1);

COMMIT;