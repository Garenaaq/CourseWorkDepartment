--------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------
CREATE TRIGGER user_info_insert_trigger
  	AFTER INSERT
  	ON authorization_info
 	FOR EACH ROW
  	EXECUTE PROCEDURE user_info_insert_trigger_fnc();

CREATE OR REPLACE FUNCTION user_info_insert_trigger_fnc()
	RETURNS TRIGGER
	AS $$
	BEGIN
		INSERT INTO rollback_table VALUES(DEFAULT, DEFAULT, json_build_object('id', NEW.ID), 'INSERT', 'authorization_info');
		RETURN NEW;
	END;
	$$ LANGUAGE plpgsql;
--------------------------------------------------------------------------
-------------------------------------------------------------------------
CREATE TRIGGER lecturer_insert_trigger
  	AFTER INSERT
  	ON lecturer
 	FOR EACH ROW
  	EXECUTE PROCEDURE lecturer_insert_trigger_fnc();

CREATE OR REPLACE FUNCTION lecturer_insert_trigger_fnc()
	RETURNS TRIGGER
	AS $$
	BEGIN
		INSERT INTO rollback_table VALUES(DEFAULT, DEFAULT, json_build_object('id', NEW.ID), 'INSERT', 'lecturer');
		RETURN NEW;
	END;
	$$ LANGUAGE plpgsql;
--------------------------------------------------------------------------
-------------------------------------------------------------------------
CREATE TRIGGER load_insert_trigger
  	AFTER INSERT
  	ON load
 	FOR EACH ROW
  	EXECUTE PROCEDURE load_insert_trigger_fnc();

CREATE OR REPLACE FUNCTION load_insert_trigger_fnc()
	RETURNS TRIGGER
	AS $$
	BEGIN
		INSERT INTO rollback_table VALUES(DEFAULT, DEFAULT, json_build_object('id', NEW.ID), 'INSERT', 'load');
		RETURN NEW;
	END;
	$$ LANGUAGE plpgsql;
--------------------------------------------------------------------------
-------------------------------------------------------------------------
CREATE TRIGGER post_insert_trigger
  	AFTER INSERT
  	ON post
 	FOR EACH ROW
  	EXECUTE PROCEDURE post_insert_trigger_fnc();

CREATE OR REPLACE FUNCTION post_insert_trigger_fnc()
	RETURNS TRIGGER
	AS $$
	BEGIN
		INSERT INTO rollback_table VALUES(DEFAULT, DEFAULT, json_build_object('id', NEW.ID), 'INSERT', 'post');
		RETURN NEW;
	END;
	$$ LANGUAGE plpgsql;
--------------------------------------------------------------------------
-------------------------------------------------------------------------
CREATE TRIGGER speciality_insert_trigger
  	AFTER INSERT
  	ON speciality
 	FOR EACH ROW
  	EXECUTE PROCEDURE speciality_insert_trigger_fnc();

CREATE OR REPLACE FUNCTION speciality_insert_trigger_fnc()
	RETURNS TRIGGER
	AS $$
	BEGIN
		INSERT INTO rollback_table VALUES(DEFAULT, DEFAULT, json_build_object('id', NEW.ID), 'INSERT', 'speciality');
		RETURN NEW;
	END;
	$$ LANGUAGE plpgsql;
--------------------------------------------------------------------------
-------------------------------------------------------------------------
CREATE TRIGGER squad_insert_trigger
  	AFTER INSERT
  	ON squad
 	FOR EACH ROW
  	EXECUTE PROCEDURE squad_insert_trigger_fnc();

CREATE OR REPLACE FUNCTION squad_insert_trigger_fnc()
	RETURNS TRIGGER
	AS $$
	BEGIN
		INSERT INTO rollback_table VALUES(DEFAULT, DEFAULT, json_build_object('id', NEW.ID), 'INSERT', 'squad');
		RETURN NEW;
	END;
	$$ LANGUAGE plpgsql;
--------------------------------------------------------------------------
-------------------------------------------------------------------------
CREATE TRIGGER subject_insert_trigger
  	AFTER INSERT
  	ON subject
 	FOR EACH ROW
  	EXECUTE PROCEDURE subject_insert_trigger_fnc();

CREATE OR REPLACE FUNCTION subject_insert_trigger_fnc()
	RETURNS TRIGGER
	AS $$
	BEGIN
		INSERT INTO rollback_table VALUES(DEFAULT, DEFAULT, json_build_object('id', NEW.ID), 'INSERT', 'subject');
		RETURN NEW;
	END;
	$$ LANGUAGE plpgsql;
--------------------------------------------------------------------------
-------------------------------------------------------------------------
CREATE TRIGGER subj_spec_insert_trigger
  	AFTER INSERT
  	ON subject_speciality
 	FOR EACH ROW
  	EXECUTE PROCEDURE subj_spec_insert_trigger_fnc();

CREATE OR REPLACE FUNCTION subj_spec_insert_trigger_fnc()
	RETURNS TRIGGER
	AS $$
	BEGIN
		INSERT INTO rollback_table VALUES(DEFAULT, DEFAULT, json_build_object('id', NEW.ID), 'INSERT', 'subject_speciality');
		RETURN NEW;
	END;
	$$ LANGUAGE plpgsql;
--------------------------------------------------------------------------
-------------------------------------------------------------------------
CREATE TRIGGER year_insert_trigger
  	AFTER INSERT
  	ON academic_year
 	FOR EACH ROW
  	EXECUTE PROCEDURE year_insert_trigger_fnc();

CREATE OR REPLACE FUNCTION year_insert_trigger_fnc()
	RETURNS TRIGGER
	AS $$
	BEGIN
		INSERT INTO rollback_table VALUES(DEFAULT, DEFAULT, json_build_object('id', NEW.ID), 'INSERT', 'academic_year');
		RETURN NEW;
	END;
	$$ LANGUAGE plpgsql;
--------------------------------------------------------------------------------
--------------------------УДАЛЕНИЕ----------------------------------------------
CREATE TRIGGER year_delete_trigger
  	AFTER DELETE
  	ON academic_year
 	FOR EACH ROW
  	EXECUTE PROCEDURE year_delete_trigger_fnc();

CREATE OR REPLACE FUNCTION year_delete_trigger_fnc()
	RETURNS TRIGGER
	AS $$
	BEGIN
		INSERT INTO rollback_table VALUES(DEFAULT, DEFAULT, json_build_object('id', OLD.ID, 'year', OLD.year, 'flag', OLD.delete_flag), 'DELETE', 'academic_year');
		RETURN NEW;
	END;
	$$ LANGUAGE plpgsql;
-------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------
CREATE TRIGGER user_info_delete_trigger
  	AFTER DELETE
  	ON authorization_info
 	FOR EACH ROW
  	EXECUTE PROCEDURE user_info_delete_trigger_fnc();

CREATE OR REPLACE FUNCTION user_info_delete_trigger_fnc()
	RETURNS TRIGGER
	AS $$
	BEGIN
		INSERT INTO rollback_table VALUES(DEFAULT, DEFAULT, json_build_object('id', OLD.ID, 'last_name', OLD.last_name_user, 'name', OLD.name_user, 'patronymic', OLD.patronymic_user, 'login', OLD.login, 'pass', OLD.pass, 'role', OLD.user_role), 'DELETE', 'authorization_info');
		RETURN NEW;
	END;
	$$ LANGUAGE plpgsql;
-------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------
CREATE TRIGGER lecturer_delete_trigger
  	AFTER DELETE
  	ON lecturer
 	FOR EACH ROW
  	EXECUTE PROCEDURE lecturer_delete_trigger_fnc();

CREATE OR REPLACE FUNCTION lecturer_delete_trigger_fnc()
	RETURNS TRIGGER
	AS $$
	BEGIN
		INSERT INTO rollback_table VALUES(DEFAULT, DEFAULT, json_build_object('id', OLD.ID, 'fk_post', OLD.fk_post, 'date_of_receipt', OLD.date_of_receipt, 'fk_user_info', OLD.fk_user_info, 'rate', OLD.rate, 'flag', OLD.delete_flag), 'DELETE', 'lecturer');
		RETURN NEW;
	END;
	$$ LANGUAGE plpgsql;
-------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------
CREATE TRIGGER load_delete_trigger
  	AFTER DELETE
  	ON load
 	FOR EACH ROW
  	EXECUTE PROCEDURE load_delete_trigger_fnc();

CREATE OR REPLACE FUNCTION load_delete_trigger_fnc()
	RETURNS TRIGGER
	AS $$
	BEGIN
		INSERT INTO rollback_table VALUES(DEFAULT, DEFAULT, json_build_object('id', OLD.ID, 'lecture_hours', OLD.lecture_hours, 'practice_hours', OLD.practice_hours, 'lab_hours', OLD.laboratory_hours, 'fk_lecturer', OLD.fk_lecturer, 'fk_subj_spec', OLD.fk_subj_spec, 'fk_academic_year', OLD.fk_academic_year, 'flag', OLD.delete_flag), 'DELETE', 'load');
		RETURN NEW;
	END;
	$$ LANGUAGE plpgsql;
-------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------
CREATE TRIGGER post_delete_trigger
  	AFTER DELETE
  	ON post
 	FOR EACH ROW
  	EXECUTE PROCEDURE post_delete_trigger_fnc();

CREATE OR REPLACE FUNCTION post_delete_trigger_fnc()
	RETURNS TRIGGER
	AS $$
	BEGIN
		INSERT INTO rollback_table VALUES(DEFAULT, DEFAULT, json_build_object('id', OLD.ID, 'name_post', OLD.name_post, 'lecture_hours', OLD.lecture_hours, 'practice_hours', OLD.practice_hours, 'lab_hours', OLD.laboratory_hours, 'flag', OLD.delete_flag), 'DELETE', 'post');
		RETURN NEW;
	END;
	$$ LANGUAGE plpgsql;
-------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------
CREATE TRIGGER speciality_delete_trigger
  	AFTER DELETE
  	ON speciality
 	FOR EACH ROW
  	EXECUTE PROCEDURE speciality_delete_trigger_fnc();

CREATE OR REPLACE FUNCTION speciality_delete_trigger_fnc()
	RETURNS TRIGGER
	AS $$
	BEGIN
		INSERT INTO rollback_table VALUES(DEFAULT, DEFAULT, json_build_object('id', OLD.ID, 'name_speciality', OLD.name_speciality, 'code_speciality', OLD.code_speciality, 'flag', OLD.delete_flag), 'DELETE', 'speciality');
		RETURN NEW;
	END;
	$$ LANGUAGE plpgsql;
-------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------
CREATE TRIGGER squad_delete_trigger
  	AFTER DELETE
  	ON squad
 	FOR EACH ROW
  	EXECUTE PROCEDURE squad_delete_trigger_fnc();

CREATE OR REPLACE FUNCTION squad_delete_trigger_fnc()
	RETURNS TRIGGER
	AS $$
	BEGIN
		INSERT INTO rollback_table VALUES(DEFAULT, DEFAULT, json_build_object('id', OLD.ID, 'name_squad', OLD.name_squad, 'fk_speciality', OLD.fk_speciality, 'num_students', OLD.number_students, 'course', OLD.course, 'recruitment_year', OLD.recruitment_year, 'flag', OLD.delete_flag), 'DELETE', 'squad');
		RETURN NEW;
	END;
	$$ LANGUAGE plpgsql;
-------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------
CREATE TRIGGER subject_delete_trigger
  	AFTER DELETE
  	ON subject
 	FOR EACH ROW
  	EXECUTE PROCEDURE subject_delete_trigger_fnc();

CREATE OR REPLACE FUNCTION subject_delete_trigger_fnc()
	RETURNS TRIGGER
	AS $$
	BEGIN
		INSERT INTO rollback_table VALUES(DEFAULT, DEFAULT, json_build_object('id', OLD.ID, 'name_subject', OLD.name_subject, 'flag', OLD.delete_flag), 'DELETE', 'subject');
		RETURN NEW;
	END;
	$$ LANGUAGE plpgsql;
-------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------
CREATE TRIGGER subj_spec_delete_trigger
  	AFTER DELETE
  	ON subject_speciality
 	FOR EACH ROW
  	EXECUTE PROCEDURE subj_spec_delete_trigger_fnc();

CREATE OR REPLACE FUNCTION subj_spec_delete_trigger_fnc()
	RETURNS TRIGGER
	AS $$
	BEGIN
		INSERT INTO rollback_table VALUES(DEFAULT, DEFAULT, json_build_object('id', OLD.ID, 'fk_name_subject', OLD.fk_name_subject, 'semester', OLD.semester, 'fk_speciality', OLD.fk_speciality, 'hours_lectures', OLD.hours_lectures, 'hours_practice', OLD.hours_practice, 'hours_laboratory', OLD.hours_laboratory, 'flag', OLD.delete_flag), 'DELETE', 'subject_speciality');
		RETURN NEW;
	END;
	$$ LANGUAGE plpgsql;
--------------------------------------------------------------------------------
--------------------------ОБНОВЛЕНИЕ----------------------------------------------
CREATE TRIGGER year_update_trigger
  	AFTER UPDATE
  	ON academic_year
 	FOR EACH ROW
  	EXECUTE PROCEDURE year_update_trigger_fnc();

CREATE OR REPLACE FUNCTION year_update_trigger_fnc()
	RETURNS TRIGGER
	AS $$
	DECLARE
	col_name TEXT;
	BEGIN
		INSERT INTO rollback_table VALUES(DEFAULT, DEFAULT, json_build_object('id', OLD.ID, 'year', OLD.year, 'delete_flag', OLD.delete_flag), 'UPDATE', 'academic_year');
		RETURN NEW;
	END;
	$$ LANGUAGE plpgsql;
-------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------
CREATE TRIGGER user_info_update_trigger
  	AFTER UPDATE
  	ON authorization_info
 	FOR EACH ROW
  	EXECUTE PROCEDURE user_info_update_trigger_fnc();

CREATE OR REPLACE FUNCTION user_info_update_trigger_fnc()
	RETURNS TRIGGER
	AS $$
	DECLARE
	col_name TEXT;
	BEGIN
		INSERT INTO rollback_table VALUES(DEFAULT, DEFAULT, json_build_object('id', OLD.ID, 'last_name', OLD.last_name_user, 'name', OLD.name_user, 'patronymic', OLD.patronymic_user, 'login', OLD.login, 'pass', OLD.pass, 'role', OLD.user_role), 'UPDATE', 'authorization_info');
		RETURN NEW;
	END;
	$$ LANGUAGE plpgsql;
-------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------
CREATE TRIGGER lecturer_update_trigger
  	AFTER UPDATE
  	ON lecturer
 	FOR EACH ROW
  	EXECUTE PROCEDURE lecturer_update_trigger_fnc();

CREATE OR REPLACE FUNCTION lecturer_update_trigger_fnc()
	RETURNS TRIGGER
	AS $$
	DECLARE
	col_name TEXT;
	BEGIN
		INSERT INTO rollback_table VALUES(DEFAULT, DEFAULT, json_build_object('id', OLD.ID, 'fk_post', OLD.fk_post, 'date_of_receipt', OLD.date_of_receipt, 'fk_user_info', OLD.fk_user_info, 'rate', OLD.rate, 'flag', OLD.delete_flag), 'UPDATE', 'lecturer');
		RETURN NEW;
	END;
	$$ LANGUAGE plpgsql;
-------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------
CREATE TRIGGER load_update_trigger
  	AFTER UPDATE
  	ON load
 	FOR EACH ROW
  	EXECUTE PROCEDURE load_update_trigger_fnc();

CREATE OR REPLACE FUNCTION load_update_trigger_fnc()
	RETURNS TRIGGER
	AS $$
	DECLARE
	col_name TEXT;
	BEGIN
		INSERT INTO rollback_table VALUES(DEFAULT, DEFAULT, json_build_object('id', OLD.ID, 'lecture_hours', OLD.lecture_hours, 'practice_hours', OLD.practice_hours, 'lab_hours', OLD.laboratory_hours, 'fk_lecturer', OLD.fk_lecturer, 'fk_subj_spec', OLD.fk_subj_spec, 'fk_academic_year', OLD.fk_academic_year, 'flag', OLD.delete_flag), 'UPDATE', 'load');
		RETURN NEW;
	END;
	$$ LANGUAGE plpgsql;
-------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------
CREATE TRIGGER post_update_trigger
  	AFTER UPDATE
  	ON post
 	FOR EACH ROW
  	EXECUTE PROCEDURE post_update_trigger_fnc();

CREATE OR REPLACE FUNCTION post_update_trigger_fnc()
	RETURNS TRIGGER
	AS $$
	DECLARE
	col_name TEXT;
	BEGIN
		INSERT INTO rollback_table VALUES(DEFAULT, DEFAULT, json_build_object('id', OLD.ID, 'name_post', OLD.name_post, 'lecture_hours', OLD.lecture_hours, 'practice_hours', OLD.practice_hours, 'lab_hours', OLD.laboratory_hours, 'flag', OLD.delete_flag), 'UPDATE', 'post');
		RETURN NEW;
	END;
	$$ LANGUAGE plpgsql;
-------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------
CREATE TRIGGER speciality_update_trigger
  	AFTER UPDATE
  	ON speciality
 	FOR EACH ROW
  	EXECUTE PROCEDURE speciality_update_trigger_fnc();

CREATE OR REPLACE FUNCTION speciality_update_trigger_fnc()
	RETURNS TRIGGER
	AS $$
	DECLARE
	col_name TEXT;
	BEGIN
		INSERT INTO rollback_table VALUES(DEFAULT, DEFAULT, json_build_object('id', OLD.ID, 'name_speciality', OLD.name_speciality, 'code_speciality', OLD.code_speciality, 'flag', OLD.delete_flag), 'UPDATE', 'speciality');
		RETURN NEW;
	END;
	$$ LANGUAGE plpgsql;
-------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------
CREATE TRIGGER squad_update_trigger
  	AFTER UPDATE
  	ON squad
 	FOR EACH ROW
  	EXECUTE PROCEDURE squad_update_trigger_fnc();

CREATE OR REPLACE FUNCTION squad_update_trigger_fnc()
	RETURNS TRIGGER
	AS $$
	DECLARE
	col_name TEXT;
	BEGIN
		INSERT INTO rollback_table VALUES(DEFAULT, DEFAULT, json_build_object('id', OLD.ID, 'name_squad', OLD.name_squad, 'fk_speciality', OLD.fk_speciality, 'num_students', OLD.number_students, 'course', OLD.course, 'recruitment_year', OLD.recruitment_year, 'flag', OLD.delete_flag), 'UPDATE', 'squad');
		RETURN NEW;
	END;
	$$ LANGUAGE plpgsql;
-------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------
CREATE TRIGGER subject_update_trigger
  	AFTER UPDATE
  	ON subject
 	FOR EACH ROW
  	EXECUTE PROCEDURE subject_update_trigger_fnc();

CREATE OR REPLACE FUNCTION subject_update_trigger_fnc()
	RETURNS TRIGGER
	AS $$
	BEGIN
		INSERT INTO rollback_table VALUES(DEFAULT, DEFAULT, json_build_object('id', OLD.ID, 'name_subject', OLD.name_subject, 'flag', OLD.delete_flag), 'UPDATE', 'subject');
		RETURN NEW;
	END;
	$$ LANGUAGE plpgsql;
-------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------
CREATE TRIGGER subj_spec_update_trigger
  	AFTER UPDATE
  	ON subject_speciality
 	FOR EACH ROW
  	EXECUTE PROCEDURE subj_spec_update_trigger_fnc();

CREATE OR REPLACE FUNCTION subj_spec_update_trigger_fnc()
	RETURNS TRIGGER
	AS $$
	BEGIN
		INSERT INTO rollback_table VALUES(DEFAULT, DEFAULT, json_build_object('id', OLD.ID, 'fk_name_subject', OLD.fk_name_subject, 'semester', OLD.semester, 'fk_speciality', OLD.fk_speciality, 'hours_lectures', OLD.hours_lectures, 'hours_practice', OLD.hours_practice, 'hours_laboratory', OLD.hours_laboratory, 'flag', OLD.delete_flag), 'UPDATE', 'subject_speciality');
		RETURN NEW;
	END;
	$$ LANGUAGE plpgsql;


CREATE OR REPLACE PROCEDURE func_back(time_old TIMESTAMP)
	 AS
	$BODY$
	DECLARE
		crs_my CURSOR FOR SELECT id, time, changes, method_, tab_name FROM rollback_table WHERE time_old <= time ORDER BY ID DESC;
		_id_back INTEGER;
    	_time TIMESTAMP;
    	_changes JSON;
    	_method TEXT;
    	_tab_name TEXT;
		_command TEXT;
		id_on_json INTEGER;
	BEGIN
		OPEN crs_my;
		LOOP
			FETCH crs_my INTO _id_back, _time, _changes, _method, _tab_name;
			IF NOT FOUND THEN EXIT;END IF;
			CASE 
				WHEN _method='INSERT' THEN
					id_on_json := _changes->>'id';
					_command := concat('DELETE FROM ', _tab_name, ' WHERE ID = ', id_on_json);
					EXECUTE _command;
				WHEN _method='DELETE' THEN
					CASE
						WHEN _tab_name = 'authorization_info' THEN
							_command := concat('INSERT INTO authorization_info VALUES (', _changes->>'id', ', ''', _changes->>'last_name', ''', ''', _changes->>'name', ''', ''', _changes->>'patronymic', ''', ''', _changes->>'login', ''', ''', _changes->>'pass', ''', ''', _changes->>'role', ''')');
							EXECUTE _command;
						WHEN _tab_name = 'academic_year' THEN
							_command := concat('INSERT INTO academic_year VALUES (', _changes->>'id', ', ''', _changes->>'year', ''', ', _changes->>'flag', ')');
							EXECUTE _command;
						WHEN _tab_name = 'lecturer' THEN
							_command := concat('INSERT INTO lecturer VALUES (', _changes->>'id', ', ', _changes->>'fk_post', ', ''', _changes->>'date_of_receipt', ''', ', _changes->>'fk_user_info', ', ', _changes->>'rate', ', ', _changes->>'flag', ')');
							EXECUTE _command;
						WHEN _tab_name = 'load' THEN
							_command := concat('INSERT INTO load VALUES (', _changes->>'id', ', ', _changes->>'lecture_hours', ', ', _changes->>'practice_hours', ', ', _changes->>'lab_hours', ', ', _changes->>'fk_lecturer', ', ', _changes->>'fk_subj_spec', ', ', _changes->>'fk_academic_year', ', ', _changes->>'flag', ')');
							EXECUTE _command;
						WHEN _tab_name = 'post' THEN
							_command := concat('INSERT INTO post VALUES (', _changes->>'id', ', ''', _changes->>'name_post', ''', ', _changes->>'lecture_hours', ', ', _changes->>'practice_hours', ', ', _changes->>'lab_hours', ', ', _changes->>'flag', ')');
							EXECUTE _command;
						WHEN _tab_name = 'speciality' THEN
							_command := concat('INSERT INTO speciality VALUES (', _changes->>'id', ', ''', _changes->>'name_speciality', ''', ''', _changes->>'code_speciality', ''', ', _changes->>'flag', ')');
							EXECUTE _command;
						WHEN _tab_name = 'squad' THEN
							_command := concat('INSERT INTO squad VALUES (', _changes->>'id', ', ''', _changes->>'name_squad', ''', ', _changes->>'fk_speciality', ', ', _changes->>'num_students', ', ', _changes->>'course', ', ''', _changes->>'recruitment_year', ''', ', _changes->>'flag', ')');
							EXECUTE _command;
						WHEN _tab_name = 'subject' THEN
							_command := concat('INSERT INTO subject VALUES (', _changes->>'id', ', ''', _changes->>'name_subject', ''', ', _changes->>'flag', ')');
							EXECUTE _command;
						WHEN _tab_name = 'subject_speciality' THEN
							_command := concat('INSERT INTO subject_speciality VALUES (', _changes->>'id', ', ', _changes->>'fk_name_subject', ', ', _changes->>'semester', ', ', _changes->>'fk_speciality', ', ', _changes->>'hours_lectures', ', ', _changes->>'hours_practice', ', ', _changes->>'hours_laboratory', ', ', _changes->>'flag', ')');
							EXECUTE _command;
					END CASE;
				WHEN _method='UPDATE' THEN
					CASE
						WHEN _tab_name = 'authorization_info' THEN
							_command := concat('UPDATE authorization_info SET last_name_user = ''', _changes->>'last_name', ''', name_user = ''', _changes->>'name', ''', patronymic_user = ''', _changes->>'patronymic', ''', login = ''', _changes->>'login', ''', pass = ''', _changes->>'pass', ''', user_role = ''', _changes->>'role', ''' WHERE ID = ', _changes->>'id');
							EXECUTE _command;
						WHEN _tab_name = 'academic_year' THEN
							_command := concat('UPDATE academic_year SET year = ''', _changes->>'year', ''', delete_flag = ', _changes->>'delete_flag', ' WHERE ID = ', _changes->>'id');
							EXECUTE _command;
						WHEN _tab_name = 'lecturer' THEN
							_command := concat('UPDATE lecturer SET fk_post = ', _changes->>'fk_post', ', date_of_receipt = ''', _changes->>'date_of_receipt', ''', fk_user_info = ', _changes->>'fk_user_info', ', rate = ', _changes->>'rate', ', delete_flag = ', _changes->>'flag', ' WHERE ID = ', _changes->>'id');
							EXECUTE _command;
						WHEN _tab_name = 'load' THEN
							_command := concat('UPDATE load SET lecture_hours = ', _changes->>'lecture_hours', ', practice_hours = ', _changes->>'practice_hours', ', laboratory_hours = ', _changes->>'lab_hours', ', fk_lecturer = ', _changes->>'fk_lecturer', ', fk_subj_spec = ', _changes->>'fk_subj_spec', ', fk_academic_year = ', _changes->>'fk_academic_year', ', delete_flag = ', _changes->>'flag', ' WHERE ID = ', _changes->>'id');
							EXECUTE _command;
						WHEN _tab_name = 'post' THEN
							_command := concat('UPDATE post SET name_post = ''', _changes->>'name_post', ''', lecture_hours = ', _changes->>'lecture_hours', ', practice_hours = ', _changes->>'practice_hours', ', laboratory_hours = ', _changes->>'lab_hours', ', delete_flag = ', _changes->>'flag', ' WHERE ID = ', _changes->>'id');
							EXECUTE _command;
						WHEN _tab_name = 'speciality' THEN
							_command := concat('UPDATE speciality SET name_speciality = ''', _changes->>'name_speciality', ''', code_speciality = ''', _changes->>'code_speciality', ', delete_flag = ', _changes->>'flag', ' WHERE ID = ', _changes->>'id');
							EXECUTE _command;
						WHEN _tab_name = 'squad' THEN
							_command := concat('UPDATE squad SET name_squad = ''', _changes->>'name_squad', ''', fk_speciality = ', _changes->>'fk_speciality', ', number_students = ', _changes->>'num_students', ', course = ', _changes->>'course', ', recruitment_year = ''', _changes->>'recruitment_year', ''', delete_flag = ', _changes->>'flag', ' WHERE ID = ', _changes->>'id');
							EXECUTE _command;
						WHEN _tab_name = 'subject' THEN
							_command := concat('UPDATE subject SET name_subject = ''', _changes->>'name_subject', ''', delete_flag = ', _changes->>'flag', ' WHERE ID = ', _changes->>'id');
							EXECUTE _command;
						WHEN _tab_name = 'subject_speciality' THEN
							_command := concat('UPDATE subject_speciality SET fk_name_subject = ', _changes->>'fk_name_subject', ', semester = ', _changes->>'semester', ', fk_speciality = ', _changes->>'fk_speciality', ', hours_lectures = ', _changes->>'hours_lectures', ', hours_practice = ', _changes->>'hours_practice', ', hours_laboratory = ', _changes->>'hours_laboratory', ', delete_flag = ', _changes->>'flag', ' WHERE ID = ', _changes->>'id');
							EXECUTE _command;
					END CASE;
			END CASE;
			DELETE FROM rollback_table WHERE ID = _id_back;
-- 			SELECT MAX(ID) FROM rollback_table INTO _id_back;
-- 			DELETE FROM rollback_table WHERE ID = _id_back;
		END LOOP;
		CLOSE crs_my;
	END;
	$BODY$
		LANGUAGE plpgsql

CREATE OR REPLACE PROCEDURE func_back_num(num INTEGER)
	 AS
	$BODY$
	DECLARE
		crs_my CURSOR FOR SELECT id, time, changes, method_, tab_name FROM rollback_table ORDER BY ID DESC LIMIT(num);
		_id_back INTEGER;
    	_time TIMESTAMP;
    	_changes JSON;
    	_method TEXT;
    	_tab_name TEXT;
		_command TEXT;
		id_on_json INTEGER;
	BEGIN
		OPEN crs_my;
		LOOP
			FETCH crs_my INTO _id_back, _time, _changes, _method, _tab_name;
			IF NOT FOUND THEN EXIT;END IF;
			CASE 
				WHEN _method='INSERT' THEN
					id_on_json := _changes->>'id';
					_command := concat('DELETE FROM ', _tab_name, ' WHERE ID = ', id_on_json);
					EXECUTE _command;
				WHEN _method='DELETE' THEN
					CASE
						WHEN _tab_name = 'authorization_info' THEN
							_command := concat('INSERT INTO authorization_info VALUES (', _changes->>'id', ', ''', _changes->>'last_name', ''', ''', _changes->>'name', ''', ''', _changes->>'patronymic', ''', ''', _changes->>'login', ''', ''', _changes->>'pass', ''', ''', _changes->>'role', ''')');
							EXECUTE _command;
						WHEN _tab_name = 'academic_year' THEN
							_command := concat('INSERT INTO academic_year VALUES (', _changes->>'id', ', ''', _changes->>'year', ''', ', _changes->>'flag', ')');
							EXECUTE _command;
						WHEN _tab_name = 'lecturer' THEN
							_command := concat('INSERT INTO lecturer VALUES (', _changes->>'id', ', ', _changes->>'fk_post', ', ''', _changes->>'date_of_receipt', ''', ', _changes->>'fk_user_info', ', ', _changes->>'rate', ', ', _changes->>'flag', ')');
							EXECUTE _command;
						WHEN _tab_name = 'load' THEN
							_command := concat('INSERT INTO load VALUES (', _changes->>'id', ', ', _changes->>'lecture_hours', ', ', _changes->>'practice_hours', ', ', _changes->>'lab_hours', ', ', _changes->>'fk_lecturer', ', ', _changes->>'fk_subj_spec', ', ', _changes->>'fk_academic_year', ', ', _changes->>'flag', ')');
							EXECUTE _command;
						WHEN _tab_name = 'post' THEN
							_command := concat('INSERT INTO post VALUES (', _changes->>'id', ', ''', _changes->>'name_post', ''', ', _changes->>'lecture_hours', ', ', _changes->>'practice_hours', ', ', _changes->>'lab_hours', ', ', _changes->>'flag', ')');
							EXECUTE _command;
						WHEN _tab_name = 'speciality' THEN
							_command := concat('INSERT INTO speciality VALUES (', _changes->>'id', ', ''', _changes->>'name_speciality', ''', ''', _changes->>'code_speciality', ''', ', _changes->>'flag', ')');
							EXECUTE _command;
						WHEN _tab_name = 'squad' THEN
							_command := concat('INSERT INTO squad VALUES (', _changes->>'id', ', ''', _changes->>'name_squad', ''', ', _changes->>'fk_speciality', ', ', _changes->>'num_students', ', ', _changes->>'course', ', ''', _changes->>'recruitment_year', ''', ', _changes->>'flag', ')');
							EXECUTE _command;
						WHEN _tab_name = 'subject' THEN
							_command := concat('INSERT INTO subject VALUES (', _changes->>'id', ', ''', _changes->>'name_subject', ''', ', _changes->>'flag', ')');
							EXECUTE _command;
						WHEN _tab_name = 'subject_speciality' THEN
							_command := concat('INSERT INTO subject_speciality VALUES (', _changes->>'id', ', ', _changes->>'fk_name_subject', ', ', _changes->>'semester', ', ', _changes->>'fk_speciality', ', ', _changes->>'hours_lectures', ', ', _changes->>'hours_practice', ', ', _changes->>'hours_laboratory', ', ', _changes->>'flag', ')');
							EXECUTE _command;
					END CASE;
				WHEN _method='UPDATE' THEN
					CASE
						WHEN _tab_name = 'authorization_info' THEN
							_command := concat('UPDATE authorization_info SET last_name_user = ''', _changes->>'last_name', ''', name_user = ''', _changes->>'name', ''', patronymic_user = ''', _changes->>'patronymic', ''', login = ''', _changes->>'login', ''', pass = ''', _changes->>'pass', ''', user_role = ''', _changes->>'role', ''' WHERE ID = ', _changes->>'id');
							EXECUTE _command;
						WHEN _tab_name = 'academic_year' THEN
							_command := concat('UPDATE academic_year SET year = ''', _changes->>'year', ''', delete_flag = ', _changes->>'delete_flag', ' WHERE ID = ', _changes->>'id');
							EXECUTE _command;
						WHEN _tab_name = 'lecturer' THEN
							_command := concat('UPDATE lecturer SET fk_post = ', _changes->>'fk_post', ', date_of_receipt = ''', _changes->>'date_of_receipt', ''', fk_user_info = ', _changes->>'fk_user_info', ', rate = ', _changes->>'rate', ', delete_flag = ', _changes->>'flag', ' WHERE ID = ', _changes->>'id');
							EXECUTE _command;
						WHEN _tab_name = 'load' THEN
							_command := concat('UPDATE load SET lecture_hours = ', _changes->>'lecture_hours', ', practice_hours = ', _changes->>'practice_hours', ', laboratory_hours = ', _changes->>'lab_hours', ', fk_lecturer = ', _changes->>'fk_lecturer', ', fk_subj_spec = ', _changes->>'fk_subj_spec', ', fk_academic_year = ', _changes->>'fk_academic_year', ', delete_flag = ', _changes->>'flag', ' WHERE ID = ', _changes->>'id');
							EXECUTE _command;
						WHEN _tab_name = 'post' THEN
							_command := concat('UPDATE post SET name_post = ''', _changes->>'name_post', ''', lecture_hours = ', _changes->>'lecture_hours', ', practice_hours = ', _changes->>'practice_hours', ', laboratory_hours = ', _changes->>'lab_hours', ', delete_flag = ', _changes->>'flag', ' WHERE ID = ', _changes->>'id');
							EXECUTE _command;
						WHEN _tab_name = 'speciality' THEN
							_command := concat('UPDATE speciality SET name_speciality = ''', _changes->>'name_speciality', ''', code_speciality = ''', _changes->>'code_speciality', ', delete_flag = ', _changes->>'flag', ' WHERE ID = ', _changes->>'id');
							EXECUTE _command;
						WHEN _tab_name = 'squad' THEN
							_command := concat('UPDATE squad SET name_squad = ''', _changes->>'name_squad', ''', fk_speciality = ', _changes->>'fk_speciality', ', number_students = ', _changes->>'num_students', ', course = ', _changes->>'course', ', recruitment_year = ''', _changes->>'recruitment_year', ''', delete_flag = ', _changes->>'flag', ' WHERE ID = ', _changes->>'id');
							EXECUTE _command;
						WHEN _tab_name = 'subject' THEN
							_command := concat('UPDATE subject SET name_subject = ''', _changes->>'name_subject', ''', delete_flag = ', _changes->>'flag', ' WHERE ID = ', _changes->>'id');
							EXECUTE _command;
						WHEN _tab_name = 'subject_speciality' THEN
							_command := concat('UPDATE subject_speciality SET fk_name_subject = ', _changes->>'fk_name_subject', ', semester = ', _changes->>'semester', ', fk_speciality = ', _changes->>'fk_speciality', ', hours_lectures = ', _changes->>'hours_lectures', ', hours_practice = ', _changes->>'hours_practice', ', hours_laboratory = ', _changes->>'hours_laboratory', ', delete_flag = ', _changes->>'flag', ' WHERE ID = ', _changes->>'id');
							EXECUTE _command;
					END CASE;
			END CASE;
			DELETE FROM rollback_table WHERE ID = _id_back;
-- 			SELECT MAX(ID) FROM rollback_table INTO _id_back;
-- 			DELETE FROM rollback_table WHERE ID = _id_back;
		END LOOP;
		CLOSE crs_my;
	END;
	$BODY$
		LANGUAGE plpgsql