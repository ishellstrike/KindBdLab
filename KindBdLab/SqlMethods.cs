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
            var a = new MySqlConnection(@"Server=localhost;Database=univer;Uid=root;Pwd=admin;");
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
                @"CREATE TABLE IF NOT EXISTS `groups` (
                `group_id` int(11) NOT NULL AUTO_INCREMENT,
                `room` varchar(255) NOT NULL,
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
                `date` TIMESTAMP(6),
                `type` varchar(20),
                `value` int(11),
                `children_id` int(11) NOT NULL,
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
                @"INSERT INTO `childrens` (`children_id`, `name`, `group`, `birth`, `mother_id`, `father_id`) VALUES
                (1, 'Самсонов Андрей Федорович', 1,  '2001-11-01',1,2),
                (2, 'Пономарев Александр Георгиевич', 1, '2000-01-01',3,4),
                (3, 'Емельяненко Алексей Иванович', 1, '2001-02-02',5,6),
                (4, 'Коноплев Даниил Михайлович', 2, '2000-06-02',7,8),
                (5, 'Озерова Екатерина Викторовна', 2, '2001-03-03',9,10),
                (6, 'Харченко Виктория Васильевна', 2, '2001-01-04',11,12),
                (7, 'Салфетников Игорь Вячесловович', 2, '2000-09-05',13,14),
                (8, 'Гузенко Олег Владимирович', 2, '2001-08-06',15,16),
                (9, 'Ворожейкин Никита Александрович', 3, '2003-01-07',17,18),
                (10, 'Каразия Роман Георгиевич', 3, '2002-01-08',19,20),
                (11, 'Пономарев Святослав Иванович', 3, '2003-03-09',21,22),
                (12, 'Харченко Ярополк Георгиевич', 3, '2002-01-01',23,24),
                (13, 'Емельяненко Арсений Георгиевич', 1, '2000-07-02',25,26),
                (14, 'Пономарев Геннадий Иванович', 2, '2001-01-03',27,28),
                (15, 'Харченко Джеральд Вячесловович', 3, '2002-05-04',29,30),
                (16, 'Самсонов Лука Александрович', 1, '2000-04-05',31,32);", msc)) {
                t.ExecuteNonQuery();
            }

            using (var t = new MySqlCommand(
                @"INSERT INTO `groups` (`group_id`, `room`) VALUES
                (1, 102),
                (2, 103),
                (3, 202),
                (4, 201);", msc)) {
                t.ExecuteNonQuery();
            }

            using (var t = new MySqlCommand(
                @"INSERT INTO `med` (`med_id`, `date`, `type`, `value`, children_id) VALUES
                (1, '2000-01-01', 'rost', 100, 1),
                (2, '2000-02-02', 'rost', 102, 1),
                (3, '2000-03-05', 'rost', 105, 1),
                (4, '2000-01-07', 'rost', 106, 1),
                (5, '2000-02-05', 'rost', 104, 1),
                (6, '2000-03-02', 'rost', 107, 1),
                (7, '2000-01-03', 'rost', 108, 1),
                (8, '2000-02-04', 'rost', 109, 1),
                (9, '2000-03-07', 'rost', 112, 1),
                (10, '2000-01-05', 'rost', 114, 1),
                (11, '2000-02-03', 'rost', 117, 1),
                (12, '2000-01-04', 'rost', 118, 1),
                (13, '2000-02-08', 'ves', 20, 3),
                (14, '2000-03-07', 'ves', 20, 1),
                (15, '2000-01-06', 'ves', 20, 2),
                (16, '2000-02-04', 'ves', 20, 4),
                (17, '2000-03-03', 'ves', 20, 5),
                (18, '2000-01-01', 'ves', 20, 6),
                (19, '2000-02-02', 'ves', 20, 7),
                (20, '2000-03-07', 'ves', 20, 8),
                (21, '2000-01-04', 'temp', 36, 1),
                (22, '2000-02-07', 'temp', 36, 2),
                (23, '2000-03-02', 'temp', 36, 3),
                (24, '2000-01-03', 'temp', 36, 4),
                (25, '2000-02-09', 'temp', 36, 5),
                (26, '2000-03-04', 'temp', 36, 6),
                (27, '2000-01-08', 'temp', 36, 7),
                (28, '2000-02-07', 'temp', 36, 8),
                (29, '2000-03-03', 'rost', 102, 11),
                (30, '2000-01-06', 'rost', 104, 5),
                (31, '2000-03-03', 'ves', 20, 5),
                (32, '2000-01-01', 'ves', 20, 6),
                (33, '2000-02-02', 'ves', 20, 7),
                (34, '2000-03-07', 'ves', 20, 8),
                (35, '2000-01-04', 'temp', 36, 7),
                (36, '2000-02-07', 'temp', 36, 8),
                (37, '2000-03-02', 'temp', 36, 9),
                (38, '2000-01-03', 'temp', 36, 10),
                (39, '2000-02-09', 'temp', 36, 11),
                (40, '2000-03-04', 'temp', 36, 12),
                (41, '2000-01-08', 'temp', 36, 13),
                (42, '2000-02-07', 'temp', 36, 14),
                (43, '2000-03-03', 'rost', 102, 15),
                (44, '2000-01-06', 'rost', 104, 16),
                (45, '2000-02-04', 'rost', 120, 1);", msc))
            {
                t.ExecuteNonQuery();
            }

            using (var t = new MySqlCommand(
                @"INSERT INTO `parents` (`parent_id`, `name`, `phone`) VALUES
                (1, 'Irina'     ,'386 626 177'),
                (2, 'Bronislaw', '542 513 557'),
                (3, 'Catherine', '607 829 006'),
                (4, 'Jacob',     '776 946 786'),
                (5, 'Ramiro','123 234 436'),    
                (6, 'Dementi','345 234 123'),
                (7, 'Emmanuel','789 657 345'),
                (8, 'Lolita','375 822 984'),
                (9, 'Trifon','604 318 302'),
                (10, 'Rodion','556 199 392'),
                (11, 'Ildar','345 234 456'),
                (12, 'Eva',''),
                (13, 'Alice','604 088 260 '),
                (14, 'Felix','547 260 941 '),
                (15, 'Lazarus','234 234 234'),
                (16, 'Porphyry','456 234 234'),
                (17, 'Lilith','098 234 784'),
                (18, 'Evelyn','234 123 544'),
                (19, 'Mayor','555 333 222'),
                (20, 'Bogdan','222 456 345'),
                (21, 'Milena','567 345 234'),
                (22, 'Bulat','345 567 678'),
                (23, 'Ruslan','345 234 234'),
                (24, 'Nahum','234 547 456'),
                (25, 'Gregory','604 088 260 '),
                (26, 'Akulina','547 260 941 '),
                (27, 'Rashid','604 088 260 '),
                (28, 'Jasmin','547 260 941 ');", msc))
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
