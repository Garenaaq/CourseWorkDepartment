CREATE TABLE IF NOT EXISTS authorization_info (
	ID SERIAL PRIMARY KEY,
	last_name_user VARCHAR(20) NOT NULL,
	name_user VARCHAR(15) NOT NULL,
	patronymic_user VARCHAR(20) NOT NULL,
	login VARCHAR(15) NOT NULL UNIQUE,
	pass TEXT,
	user_role VARCHAR(15) NOT NULL
);

SELECT FROM lecturer
DELETE FROM rollback_table

CREATE TABLE IF NOT EXISTS speciality (
	ID SERIAL PRIMARY KEY,
	name_speciality TEXT NOT NULL,
	code_speciality CHAR(8) NOT NULL UNIQUE,
	delete_flag INTEGER
);

CREATE TABLE IF NOT EXISTS squad (
	ID SERIAL PRIMARY KEY,
	name_squad VARCHAR(12) NOT NULL,
	fk_speciality INTEGER REFERENCES speciality(ID) ON DELETE SET NULL,
	number_students SMALLINT NOT NULL,
	course SMALLINT NOT NULL,
	recruitment_year VARCHAR(4) NOT NULL,
	delete_flag INTEGER
);


CREATE TABLE IF NOT EXISTS academic_year (
	ID SERIAL PRIMARY KEY,
	year VARCHAR(9) NOT NULL,
	delete_flag INTEGER
);


CREATE TABLE IF NOT EXISTS subject_speciality(
	ID SERIAL PRIMARY KEY,
	fk_name_subject INTEGER REFERENCES subject(ID) ON DELETE SET NULL,
	semester INTEGER NOT NULL,
	fk_speciality INTEGER REFERENCES speciality(ID) ON DELETE SET NULL,
	hours_lectures INTEGER,
	hours_practice INTEGER,
	hours_laboratory INTEGER,
	delete_flag INTEGER
);


CREATE TABLE IF NOT EXISTS post (
	ID SERIAL PRIMARY KEY,
	name_post VARCHAR(21) NOT NULL UNIQUE,
	lecture_hours INTEGER NOT NULL,
	practice_hours INTEGER NOT NULL,
	laboratory_hours INTEGER NOT NULL,
	delete_flag INTEGER
);


CREATE TABLE IF NOT EXISTS lecturer (
	ID SERIAL PRIMARY KEY,
	fk_post INTEGER REFERENCES post(ID) ON DELETE SET NULL,
	date_of_receipt DATE NOT NULL,
	fk_user_info INTEGER REFERENCES authorization_info(ID) ON DELETE SET NULL UNIQUE,
	rate REAL NOT NULL,
	delete_flag INTEGER
);


CREATE TABLE IF NOT EXISTS load (
	ID SERIAL PRIMARY KEY,
	lecture_hours INTEGER,
	practice_hours INTEGER,
	laboratory_hours INTEGER,
	fk_lecturer INTEGER REFERENCES lecturer(ID) NOT NULL,
	fk_subj_spec INTEGER REFERENCES subject_speciality(ID) NOT NULL,
	fk_academic_year INTEGER REFERENCES academic_year(ID) NOT NULL,
	delete_flag INTEGER
);


CREATE TABLE IF NOT EXISTS subject (
	ID SERIAL PRIMARY KEY,
	name_subject TEXT NOT NULL UNIQUE,
	delete_flag INTEGER
);

CREATE TABLE IF NOT EXISTS rollback_table (
	ID SERIAL PRIMARY KEY,
	time TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
	changes JSON,
	method_ TEXT,
	tab_name TEXT
);


GRANT ALL PRIVILEGES ON ALL TABLES IN SCHEMA public TO admin;
REVOKE ALL ON public.authorization_info FROM admin;
GRANT USAGE, SELECT ON ALL SEQUENCES IN SCHEMA public TO admin;

GRANT INSERT, UPDATE, SELECT ON ALL TABLES IN SCHEMA public TO admin;
GRANT USAGE ON ALL SEQUENCES IN SCHEMA public TO admin;
GRANT EXECUTE ON ALL FUNCTIONS IN SCHEMA public TO admin;
GRANT SELECT ON academic_year, lecturer, load, post, speciality, squad, subject, subject_speciality TO lecturer, head_chair;
GRANT SELECT (id, last_name_user, name_user, patronymic_user) ON TABLE authorization_info TO lecturer, head_chair;
---------------------------------------------------------------------------
---------------------------------------------------------------------------




-- Добавляет новых преподавателей 
CREATE PROCEDURE insert_lecturer(
	fk_post TEXT, date_of_receipt DATE,
	 fk_user_info TEXT, rate REAL, del INTEGER)
	LANGUAGE SQL
	AS $$
	INSERT INTO lecturer VALUES 
	(DEFAULT, (SELECT post.ID FROM post WHERE name_post = fk_post), date_of_receipt, 
	(SELECT authorization_info.ID FROM authorization_info WHERE login = fk_user_info), rate, del);
	$$;
----------------
CREATE PROCEDURE update_lecturer_info(name_p VARCHAR(21), date_of_rec DATE, id_user INTEGER, rat REAL)
	LANGUAGE SQL
	AS $$
	UPDATE lecturer SET fk_post = (SELECT ID FROM post WHERE name_post = name_p),
    date_of_receipt = date_of_rec, rate = rat
    WHERE ID = id_user
	$$;
-----------------------
CREATE PROCEDURE update_squad_info(name_sq TEXT, name_spec TEXT, 
								  students_num INTEGER, cour INTEGER, year VARCHAR(4), id_squad INTEGER)
	LANGUAGE SQL
	AS $$
	UPDATE squad SET name_squad = name_sq, fk_speciality = (SELECT ID FROM speciality WHERE
                                name_speciality = name_spec), 
								number_students = students_num, course = cour, recruitment_year = year WHERE ID = id_squad
	$$;
-------------------------
CREATE PROCEDURE update_subject_speciality_info(name_sub TEXT, semestr INTEGER, name_spec TEXT, 
								  lecturer_hours INTEGER, practice_hours INTEGER, 
									laboratory_hours INTEGER, i INTEGER)
	LANGUAGE SQL
	AS $$
	UPDATE subject_speciality SET fk_name_subject = (SELECT ID FROM subject WHERE name_subject = name_sub),
    semester = semestr, fk_speciality = (SELECT ID FROM speciality WHERE name_speciality = name_spec),
    hours_lectures = lecturer_hours, hours_practice = practice_hours, hours_laboratory = laboratory_hours WHERE ID = i
	$$;
------------------------------
CREATE OR REPLACE FUNCTION select_num_group_speciality(name_spec TEXT)
    RETURNS TABLE
            (
                count BIGINT,
				SUM BIGINT
            )
    LANGUAGE plpgsql
AS
$$
BEGIN
    RETURN QUERY (
      SELECT COUNT(squad.name_squad), SUM(squad.number_students) FROM squad WHERE 
	  fk_speciality = (SELECT ID FROM speciality WHERE name_speciality = name_spec) AND squad.delete_flag = 0
    );
END;
$$;
