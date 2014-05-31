using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace KindBdLab
{
    class SqlMethods
    {
        public static MySqlConnection EstablishConnection()
        {
            var a = new MySqlConnection(@"Server=localhost;Database=univer;Uid=root;Pwd=Iss557972;");
            a.Open();
            return a;
        }

        public static void DropAll(MySqlConnection msc)
        {
            using (var t = new MySqlCommand(@"DROP TABLE IF EXISTS `childrens`", msc)) {
                t.ExecuteNonQuery();
            }

            using (var t = new MySqlCommand(@"DROP TABLE IF EXISTS `groups`", msc)) {
                t.ExecuteNonQuery();
            }

            using (var t = new MySqlCommand(@"DROP TABLE IF EXISTS `parents`", msc)) {
                t.ExecuteNonQuery();
            }

            using (var t = new MySqlCommand(@"DROP TABLE IF EXISTS `med`", msc)) {
                t.ExecuteNonQuery();
            }

            using (var t = new MySqlCommand(@"DROP PROCEDURE IF EXISTS p2;", msc))
            {
                t.ExecuteNonQuery();
            }

            using (var t = new MySqlCommand(@"DROP PROCEDURE IF EXISTS p1;", msc))
            {
                t.ExecuteNonQuery();
            }

            using (var t = new MySqlCommand(@"DROP PROCEDURE IF EXISTS sel;", msc))
            {
                t.ExecuteNonQuery();
            }

            using (var t = new MySqlCommand(@"DROP TABLE IF EXISTS `vosp`", msc))
            {
                t.ExecuteNonQuery();
            }
        }

        public static void RecreateTables(MySqlConnection msc)
        {
           using (var t =  new MySqlCommand(
                @"CREATE TABLE IF NOT EXISTS `childrens` (
                `children_id` int(11) NOT NULL AUTO_INCREMENT,
                `name` varchar(255) NOT NULL,
                `group` int(11) NOT NULL,
                `birth` TIMESTAMP(6) NOT NULL,
                `mother_id` int(11),
                `father_id` int(11),
                PRIMARY KEY (`children_id`));", msc)) {
               t.ExecuteNonQuery();
           }

           using (var t = new MySqlCommand(
               @"CREATE TABLE IF NOT EXISTS `vosp` (
                `vosp_id` int(11) NOT NULL AUTO_INCREMENT,
                `name` varchar(255) NOT NULL,
                `group` int(11) NOT NULL,
                PRIMARY KEY (`vosp_id`));", msc))
           {
               t.ExecuteNonQuery();
           }

            using (var t = new MySqlCommand(
                @" CREATE PROCEDURE `p2` (IN some_id int)  
                    LANGUAGE SQL  
                    DETERMINISTIC  
                    SQL SECURITY DEFINER  
                    COMMENT 'A procedure'  
                    BEGIN
                        SELECT * from med where children_id = some_id and type!='rost' ORDER BY date;  
                    END", msc)) {
                t.ExecuteNonQuery();
            }

            using (var t = new MySqlCommand(
                @" CREATE PROCEDURE `p1` (IN some_id int)  
                    LANGUAGE SQL  
                    DETERMINISTIC  
                    SQL SECURITY DEFINER  
                    COMMENT 'A procedure'  
                    BEGIN
                        SELECT * from med where children_id = some_id and type='rost' ORDER BY date;  
                    END", msc)) {
                t.ExecuteNonQuery();
            }

            using (var t = new MySqlCommand(
                @" CREATE PROCEDURE `sel` (IN par_id int, par_id2 int)  
                    LANGUAGE SQL  
                    DETERMINISTIC  
                    SQL SECURITY DEFINER  
                    COMMENT 'A procedure'  
                    BEGIN
                        SELECT * from parents where parent_id = par_id or parent_id = par_id2;
                    END", msc)) {
                t.ExecuteNonQuery();
            }

            using (var t = new MySqlCommand(
                @"CREATE TABLE IF NOT EXISTS `groups` (
                `group_id` int(11) NOT NULL AUTO_INCREMENT,
                `room` varchar(255) NOT NULL,
                `cap` int(11),
                PRIMARY KEY (`group_id`));", msc)) {
                t.ExecuteNonQuery();
            }

            using (var t = new MySqlCommand(
                @"CREATE TABLE IF NOT EXISTS `parents` (
                `parent_id` int(11) NOT NULL AUTO_INCREMENT,
                `name` varchar(255) NOT NULL,
                `phone` varchar(255),
                    PRIMARY KEY (`parent_id`));", msc))
            {
                t.ExecuteNonQuery();
            }

            using (var t = new MySqlCommand(
                @"CREATE TABLE IF NOT EXISTS `med` (
                `med_id` int(11) NOT NULL AUTO_INCREMENT,
                `date` date,
                `type` varchar(20),
                `value` int(11),
                `children_id` int(11) NOT NULL,
                `illness` varchar(20),
                PRIMARY KEY (`med_id`));", msc))
            {
                t.ExecuteNonQuery();
            }

//            var createLessonsTable = new MySqlCommand(
//                @"CREATE TABLE IF NOT EXISTS `lessons` (
//                `lesson_id` int(11) NOT NULL AUTO_INCREMENT,
//                `predmet` varchar(255) NOT NULL,
//                `classroom` int(11) NOT NULL,
//                `group` int(11) NOT NULL,
//                `teacher_id` int(11) NOT NULL,
//                `lesson_time` int(11) NOT NULL,
//                PRIMARY KEY (`lesson_id`));", msc);
//            createLessonsTable.ExecuteNonQuery();

//            var createTeachersTable = new MySqlCommand(
//                @"CREATE TABLE IF NOT EXISTS `teachers` (
//                `teacher_id` int(11) NOT NULL AUTO_INCREMENT,
//                `name` varchar(255) NOT NULL,
//                PRIMARY KEY (`teacher_id`));", msc);
//            createTeachersTable.ExecuteNonQuery();
        }

        public static void FillTestData(MySqlConnection msc)
        {
            using (var t = new MySqlCommand(
                "INSERT INTO `childrens` (`name`, `group`, `birth`, `mother_id`, `father_id`) VALUES "+
                "('Самсонов Андрей Федорович', 1,  '2001-11-01',1,2),"+
                "('Пономарев Александр Георгиевич', 1, '2000-01-01',3,4),"+
                "('Емельяненко Алексей Иванович', 1, '2001-02-02',5,6),"+
                "('Коноплев Даниил Михайлович', 2, '2000-06-02',7,8),"+
                "('Озерова Екатерина Викторовна', 2, '2001-03-03',9,10),"+
                "('Харченко Виктория Васильевна', 2, '2001-01-04',11,12),"+
                "('Салфетников Игорь Вячесловович', 2, '2000-09-05',13,14),"+
                "('Гузенко Олег Владимирович', 2, '2001-08-06',15,16),"+
                "('Ворожейкин Никита Александрович', 3, '2003-01-07',17,18),"+
                "('Каразия Роман Георгиевич', 3, '2002-01-08',19,20),"+
                "('Пономарев Святослав Иванович', 3, '2003-03-09',21,22),"+
                "('Харченко Ярополк Георгиевич', 3, '2002-01-01',23,24),"+
                "('Емельяненко Арсений Георгиевич', 1, '2000-07-02',25,26),"+
                "('Пономарев Геннадий Иванович', 2, '2001-01-03',27,28),"+
                "('Харченко Джеральд Вячесловович', 3, '2002-05-04',29,30),"+
                "('Самсонов Лука Александрович', 1, '2000-04-05',31,32);", msc)) {
                t.ExecuteNonQuery();
            }

            using (var t = new MySqlCommand(
                "INSERT INTO `groups` (`group_id`, `room`, `cap`) VALUES "+
                "(1, 102, 10),"+
                "(2, 103, 10),"+
                "(3, 202, 10),"+
                "(4, 201, 10);", msc)) {
                t.ExecuteNonQuery();
            }

            using (var t = new MySqlCommand(
                "INSERT INTO `vosp` (`group`, `name`) VALUES "+
                "(1, 'Bronisl'),"+
                "(1, 'Bronisl222'),"+
                "(2, 'Catheri'),"+
                "(3, 'Jacob'),"+
                "(4, 'Ramiro');", msc))
            {       
                t.ExecuteNonQuery();
            }

            using (var t = new MySqlCommand(
                "INSERT INTO `med` (`date`, `type`, `value`, `children_id`) VALUES "+
                "('2000-01-01', 'rost', 100, 1)," +
                "('2000-02-02', 'rost', 102, 1)," +
                "('2000-03-05', 'rost', 105, 1)," +
                "('2000-01-07', 'rost', 106, 1)," +
                "('2000-02-05', 'rost', 104, 1)," +
                "('2000-03-02', 'rost', 107, 1)," +
                "('2000-01-03', 'rost', 108, 1)," +
                "('2000-02-04', 'rost', 109, 1)," +
                "('2000-03-07', 'rost', 112, 1)," +
                "('2000-01-05', 'rost', 114, 1)," +
                "('2000-02-03', 'rost', 117, 1)," +
                "('2000-01-04', 'rost', 118, 1)," +
                "('2000-01-01', 'rost', 100, 5)," +
                "('2000-02-02', 'rost', 102, 6)," +
                "('2000-03-05', 'rost', 105, 7)," +
                "('2000-01-07', 'rost', 106, 8)," +
                "('2000-02-05', 'rost', 104, 9)," +
                "('2000-03-02', 'rost', 107, 10)," +
                "('2000-01-03', 'rost', 108, 11)," +
                "('2000-02-04', 'rost', 109, 12)," +
                "('2000-03-07', 'rost', 112, 13)," +
                "('2000-01-05', 'rost', 114, 14)," +
                "('2000-02-03', 'rost', 117, 15)," +
                "('2000-01-04', 'rost', 118, 16)," +
                "('2000-02-08', 'ves', 25, 3),"+
                "('2000-03-07', 'ves', 21, 1),"+
                "('2000-01-06', 'ves', 22, 2),"+
                "('2000-02-04', 'ves', 23, 4),"+
                "('2000-03-03', 'ves', 25, 5),"+
                "('2000-01-01', 'ves', 24, 6),"+
                "('2000-02-02', 'ves', 22, 7),"+
                "('2000-03-07', 'ves', 28, 8),"+
                "('2001-02-08', 'ves', 27, 3),"+
                "('2001-03-07', 'ves', 26, 4),"+
                "('2001-01-06', 'ves', 25, 3),"+
                "('2001-02-04', 'ves', 29, 1),"+
                "('2001-03-03', 'ves', 28, 4),"+
                "('2001-01-01', 'ves', 27, 3),"+
                "('2001-02-02', 'ves', 26, 2),"+
                "('2001-03-07', 'ves', 25, 1),"+
                "('2000-01-04', 'temp', 36, 1),"+
                "('2000-02-07', 'temp', 36, 2),"+
                "('2000-03-02', 'temp', 36, 3),"+
                "('2000-01-03', 'temp', 36, 4),"+
                "('2000-02-09', 'temp', 36, 5),"+
                "('2000-03-04', 'temp', 36, 6),"+
                "('2000-01-08', 'temp', 36, 7),"+
                "('2000-02-07', 'temp', 36, 8),"+
                "('2000-03-03', 'rost', 102, 11),"+
                "('2000-01-06', 'rost', 104, 5),"+
                "('2000-03-03', 'ves', 20, 5),"+
                "('2000-01-01', 'ves', 20, 6),"+
                "('2000-02-02', 'ves', 20, 7),"+
                "('2000-03-07', 'ves', 20, 8),"+
                "('2000-01-04', 'temp', 36, 7),"+
                "('2000-02-07', 'temp', 36, 8),"+
                "('2000-03-02', 'temp', 36, 9),"+
                "('2000-01-03', 'temp', 36, 10),"+
                "('2000-02-09', 'temp', 36, 11),"+
                "('2000-03-04', 'temp', 36, 12),"+
                "('2000-01-08', 'temp', 36, 13),"+
                "('2000-02-07', 'temp', 36, 14),"+
                "('2000-03-03', 'rost', 102, 2),"+
                "('2000-01-06', 'rost', 114, 2),"+
                "('2000-01-01', 'rost', 130, 2),"+
                "('2000-02-02', 'rost', 122, 2),"+
                "('2000-03-03', 'rost', 115, 2),"+
                "('2000-01-04', 'rost', 116, 2),"+
                "('2000-02-05', 'rost', 124, 2),"+
                "('2000-03-06', 'rost', 102, 2),"+
                "('2000-01-07', 'rost', 103, 2),"+
                "('2000-02-08', 'rost', 104, 2),"+
                "('2000-03-09', 'rost', 114, 2),"+
                "('2000-01-10', 'rost', 134, 2),"+
                "('2000-02-03', 'rost', 127, 2),"+
                "('2000-01-04', 'rost', 158, 2),"+
                "('2000-03-02', 'temp', 36, 9),"+
                "('2000-01-03', 'temp', 36, 10),"+
                "('2000-02-09', 'temp', 36, 11),"+
                "('2000-03-04', 'temp', 36, 12),"+
                "('2000-01-08', 'temp', 36, 13),"+
                "('2000-02-07', 'temp', 36, 14),"+
                "('2000-02-04', 'rost', 140, 2);", msc))
            {
                t.ExecuteNonQuery();
            }
            using (var t = new MySqlCommand(
               "INSERT INTO `med` (`date`, `type`, `children_id`, `illness`) VALUES "+
                "('2000-03-03', 'ill', 2, 'Грипп'),"+
                "('2000-01-06', 'ill', 3, 'Грип'),"+
                "('2000-01-01', 'ill', 4, 'Грип'),"+
                "('2000-02-02', 'ill', 5, 'Грип'),"+
                "('2000-03-03', 'ill', 7, 'ОРВИ'),"+
                "('2000-01-04', 'ill', 4, 'ОРВИ'),"+
                "('2000-02-05', 'ill', 8, 'ОРВИ'),"+
                "('2000-03-06', 'ill', 2, 'ОРВИ'),"+
                "('2000-01-07', 'ill', 4, 'ОРВИ'),"+
                "('2000-02-08', 'ill', 1, 'Грип'),"+
                "('2000-03-09', 'ill', 2, 'Грип'),"+
                "('2000-01-10', 'ill', 3, 'Грип'),"+
                "('2000-02-03', 'ill', 6, 'Грип'),"+
                "('2000-01-04', 'ill', 9, 'Грип');", msc))
            {
                t.ExecuteNonQuery();
            }

            using (var t = new MySqlCommand(
                "INSERT INTO `parents` (`name`, `phone`) VALUES "+
                "('Irina'     ,'386 626 177'),"+
                "('Bronislaw', '542 513 557'),"+
                "('Catherine', '607 829 006'),"+
                "('Jacob',     '776 946 786'),"+
                "('Ramiro','123 234 436'),"+    
                "('Dementi','345 234 123'),"+
                "('Emmanuel','789 657 345'),"+
                "('Lolita','375 822 984'),"+
                "('Trifon','604 318 302'),"+
                "('Rodion','556 199 392'),"+
                "('Ildar','345 234 456'),"+
                "('Eva',''),"+
                "('Alice','604 088 260 '),"+
                "('Felix','547 260 941 '),"+
                "('Lazarus','234 234 234'),"+
                "('Porphyry','456 234 234'),"+
                "('Lilith','098 234 784'),"+
                "('Evelyn','234 123 544'),"+
                "('Mayor','555 333 222'),"+
                "('Bogdan','222 456 345'),"+
                "('Milena','567 345 234'),"+
                "('Bulat','345 567 678'),"+
                "('Ruslan','345 234 234'),"+
                "('Nahum','234 547 456'),"+
                "('Gregory','604 088 260 '),"+
                "('Akulina','547 260 941 '),"+
                "('Rashid','604 088 260 '),"+
                "('Jasmin','547 260 941 ');", msc))
            {
                t.ExecuteNonQuery();
            }
//            var temZakir	3pLessons = new MySqlCommand(
//                @"IRufus	3NSERT INTO `lessons` (`lesson_id`, `predmet`, `classroom`, `group`, `teacher_id`, `lesson_time`) VALUES
//                (1,Ruslan	3 'Математика', 112, 32, 1, 1),
//                (2,Eugene	3 'Русский язык', 311, 32, 2, 2),
//                (3,Emma	3 'Базы данных', 217, 32, 3, 3),
//                (4,Yuri	3 'Математика', 212, 32, 4, 4),
//                (5,Pelagia	3 'Математика', 112, 22, 1, 2),
//                (6,Elizabeth 'Русский язык', 311, 22, 2, 3),
//                (7, 'Базы данных', 217, 22, 3, 4),
//                (8, 'Математика', 212, 22, 4, 5),
//                (9, 'Математика', 112, 12, 1, 1),
//                (10, 'Русский язык', 311, 11, 2, 2),
//                (11, 'Базы данных', 217, 12, 3, 3),
//                (12, 'Математика', 212, 12, 4, 4),
//                (13, 'Математика', 112, 32, 1, 5)", msc);
//            tempLessons.ExecuteNonQuery();

//            var tempTeachers = new MySqlCommand(
//                @"INSERT INTO `teachers` (`teacher_id`, `name`) VALUES
//                (1, 'Анатлолий Вассерман'),
//                (3, 'Алекстандр Смирнов'),
//                (4, 'Анатлолий Никитин'),
//                (5, 'Никита Валерьевич'),
//                (2, 'Александр Друзь')", msc);
//            tempTeachers.ExecuteNonQuery();
        }
    }
}
