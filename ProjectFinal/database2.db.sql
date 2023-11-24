BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "role" (
	"idrole"	INTEGER,
	"role"	TEXT NOT NULL UNIQUE,
	PRIMARY KEY("idrole" AUTOINCREMENT)
);
CREATE TABLE IF NOT EXISTS "student" (
	"dateOfBirth"	DATE NOT NULL DEFAULT '1977-01-01',
	"nationality"	INTEGER NOT NULL,
	"address"	TEXT NOT NULL,
	"user_id"	INTEGER NOT NULL,
	FOREIGN KEY("user_id") REFERENCES "user"("id") ON DELETE CASCADE ON UPDATE CASCADE,
	PRIMARY KEY("user_id")
);
CREATE TABLE IF NOT EXISTS "subject" (
	"idsubject"	INTEGER NOT NULL,
	"sunbjectName"	TEXT NOT NULL,
	"description"	TEXT,
	PRIMARY KEY("idsubject")
);
CREATE TABLE IF NOT EXISTS "subject_has_user" (
	"subject_idsubject"	INTEGER NOT NULL,
	"user_id"	INTEGER NOT NULL,
	"YearCoursed"	TEXT NOT NULL,
	"credits"	INTEGER NOT NULL,
	FOREIGN KEY("subject_idsubject") REFERENCES "subject"("idsubject") ON DELETE CASCADE ON UPDATE CASCADE,
	FOREIGN KEY("user_id") REFERENCES "user"("id") ON DELETE CASCADE ON UPDATE CASCADE,
	PRIMARY KEY("subject_idsubject","user_id")
);
CREATE TABLE IF NOT EXISTS "user" (
	"name"	TEXT NOT NULL,
	"surrname"	TEXT NOT NULL,
	"password"	TEXT NOT NULL,
	"id"	INTEGER,
	"username"	TEXT NOT NULL UNIQUE,
	"role_idrole"	INTEGER NOT NULL,
	FOREIGN KEY("role_idrole") REFERENCES "role"("idrole") ON DELETE CASCADE ON UPDATE CASCADE,
	PRIMARY KEY("id" AUTOINCREMENT)
);
INSERT INTO "role" VALUES (1,'admin');
INSERT INTO "role" VALUES (2,'profesor');
INSERT INTO "role" VALUES (3,'user');
INSERT INTO "user" VALUES ('hola','putos','1234',0,'admin',1);
INSERT INTO "user" VALUES ('Jerez','Frontera','1234',1,'andalu',3);
INSERT INTO "user" VALUES ('Profesor','Profesorez','1234',2,'prof',2);
CREATE INDEX IF NOT EXISTS "idx_subject_has_user_user_id" ON "subject_has_user" (
	"user_id"
);
CREATE INDEX IF NOT EXISTS "idx_subject_has_user_subject_id" ON "subject_has_user" (
	"subject_idsubject"
);
COMMIT;
