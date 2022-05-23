-- phpMyAdmin SQL Dump
-- version 4.9.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: May 23, 2022 at 09:13 PM
-- Server version: 10.4.8-MariaDB
-- PHP Version: 7.3.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `db_commission`
--

-- --------------------------------------------------------

--
-- Table structure for table `tbl_account`
--

CREATE TABLE `tbl_account` (
  `account_ID` int(11) NOT NULL,
  `account_Username` varchar(50) DEFAULT NULL,
  `account_Email` varchar(100) DEFAULT NULL,
  `account_Password` varchar(100) DEFAULT NULL,
  `account_Name` varchar(100) NOT NULL,
  `account_Department` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_account`
--

INSERT INTO `tbl_account` (`account_ID`, `account_Username`, `account_Email`, `account_Password`, `account_Name`, `account_Department`) VALUES
(1, 'admin1', 'admin1@gmail.com', '25f43b1486ad95a1398e3eeb3d83bc4010015fcc9bedb35b432e00298d5021f7', 'Engr  John Lemar Tirao', 'BSCE'),
(2, 'admin2', 'Almerol@gmail.com', '1c142b2d01aa34e9a36bde480645a57fd69e14155dacfab5a3f9257b77fdc8d8', 'Engr. Jemuel D. Almerol', 'BSEE'),
(3, 'admin', 'admin@gmail.com', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 'Mr. Patrick Luis Francisco', 'BSIT');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_curriculum`
--

CREATE TABLE `tbl_curriculum` (
  `curr_ID` int(11) NOT NULL,
  `curr_Code` varchar(50) DEFAULT NULL,
  `curr_Title` varchar(100) DEFAULT NULL,
  `curr_Units` int(11) NOT NULL,
  `curr_Pre_Req` varchar(50) NOT NULL,
  `curr_Semester` varchar(50) NOT NULL,
  `curr_Yearlevel` int(11) NOT NULL,
  `curr_Batch` int(11) DEFAULT NULL,
  `curr_Department` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_curriculum`
--

INSERT INTO `tbl_curriculum` (`curr_ID`, `curr_Code`, `curr_Title`, `curr_Units`, `curr_Pre_Req`, `curr_Semester`, `curr_Yearlevel`, `curr_Batch`, `curr_Department`) VALUES
(1, 'CC 2', 'Computer Programming 1', 2, ' ', 'First Semester', 1, 2022, 'Information Technology'),
(2, 'CC 3', 'Computer Programming 2', 2, 'CC 2', 'Second Semester', 1, 2022, 'Information Technology'),
(3, 'GE 2', 'Reading in Philippine History', 3, ' ', 'First Semester', 1, 2022, 'Information Technology'),
(4, 'GE 4', 'Mathematics in Modern World', 3, ' ', 'First Semester', 1, 2022, 'Information Technology'),
(5, 'GE 5', 'Purposive Communication', 3, ' ', 'First Semester', 1, 2022, 'Information Technology'),
(6, 'GE 6', 'Arts Appreciation', 3, ' ', 'First Semester', 1, 2022, 'Information Technology'),
(7, 'GE 7', 'Science Technology & Society', 3, ' ', 'Second Semester', 1, 2022, 'Information Technology'),
(8, 'GE 8', 'Ethics', 3, ' ', 'Second Semester', 1, 2022, 'Information Technology'),
(9, 'MS 101', 'Discrete Mathematics', 3, ' ', 'Second Semester', 1, 2022, 'Information Technology'),
(10, 'CC 4', 'Data Structures and Algorithms', 3, ' ', 'First Semester', 2, 2022, 'Information Technology'),
(11, 'DIGITAL 1', 'Digital Logic Design', 3, ' ', 'First Semester', 2, 2022, 'Information Technology'),
(12, 'MST 4', 'Living in the IT Era', 3, ' ', 'Second Semester', 2, 2022, 'Information Technology'),
(13, 'NET 101', 'Networking 1', 2, ' ', 'First Semester', 2, 2022, 'Information Technology'),
(14, 'NET 101L', 'Networking 1 L', 1, ' ', 'First Semester', 2, 2022, 'Information Technology'),
(15, 'NET 102', 'Networking 2', 2, 'NET 101', 'Second Semester', 2, 2022, 'Information Technology'),
(16, 'NET 102L', 'Networking 2 (Lab)', 1, 'NET 101L', 'Second Semester', 2, 2022, 'Information Technology'),
(17, 'RIZ', 'Life and Works of Rizal', 3, ' ', 'First Semester', 2, 2022, 'Information Technology'),
(18, 'TECHNO', 'Technopreneurship', 2, ' ', 'Second Semester', 2, 2022, 'Information Technology'),
(19, 'WS 101', 'Web Systems and Technology 1', 2, ' ', 'Second Semester', 2, 2022, 'Information Technology'),
(20, 'WS 101L', 'Web Systems and Technology 1 (Lab)', 1, ' ', 'Second Semester', 2, 2022, 'Information Technology'),
(21, 'CAP 1', 'Capstone 1', 2, 'RESEARCH 1', 'Second Semester', 3, 2022, 'Information Technology'),
(22, 'IAS', 'Information Assurance and Security 1', 3, ' ', 'Second Semester', 3, 2022, 'Information Technology'),
(23, 'IPT 101', 'Integrative Programming and Technology 1 (Game Devt 1)', 2, ' ', 'First Semester', 3, 2022, 'Information Technology'),
(24, 'IPT 101L', 'Integrative Programming and Technology 1 (Game Devt 1 Lab)', 1, ' ', 'First Semester', 3, 2022, 'Information Technology'),
(25, 'IPT 102', 'Integrative Programming and Technology 2 (Game Devt 2)', 2, 'IPT 101', 'Second Semester', 3, 2022, 'Information Technology'),
(26, 'IPT 102L', 'Integrative Programming and Technology 2 (Game Devt 2 Lab)', 1, 'IPT 101L', 'Second Semester', 3, 2022, 'Information Technology'),
(27, 'IT EL 3', 'Software Development and Testing', 2, ' ', 'Second Semester', 3, 2022, 'Information Technology'),
(28, 'RESEARCH 1', 'Methods of Research in Computing', 3, ' ', 'First Semester', 3, 2022, 'Information Technology'),
(29, 'SIA 101', 'System Integration and Architecture 1', 2, ' ', 'First Semester', 3, 2022, 'Information Technology'),
(30, 'SIA 101L', 'System Integration and Architecture 1 (Lab)', 1, ' ', 'First Semester', 3, 2022, 'Information Technology'),
(31, 'CAP 2', 'Capstone 2', 6, 'IPT 102', 'First Semester', 4, 2022, 'Information Technology'),
(32, 'OJT', 'On the Job Training (486 Hours)', 6, ' ', 'Mid Year', 4, 2022, 'Information Technology'),
(33, 'SA 101', 'System Administration and Maintenance', 2, ' ', 'First Semester', 4, 2022, 'Information Technology'),
(34, 'SA 101L', 'System Administration and Maintenance (Lab)', 1, ' ', 'First Semester', 4, 2022, 'Information Technology'),
(35, 'SEMTR', 'Seminars and Colloquia', 3, ' None', 'Second Semester', 4, 2022, 'Information Technology'),
(36, 'ST 1', 'Special Topics 1 (Network and Cisco)', 3, ' ', 'First Semester', 4, 2022, 'Information Technology'),
(37, 'ST 2', 'Special Topics 1 (Programming and Database)', 3, ' None', 'Second Semester', 4, 2022, 'Information Technology'),
(38, 'TECHNO L', 'Technopreneurship L', 1, ' None', 'Second Semester', 4, 2022, 'Information Technology'),
(39, 'GE 2', 'Readings in Philippine History', 3, 'None', 'First Semester', 1, 2018, 'Electrical Engineering'),
(40, 'GE 4', 'Mathematics in the Modern World', 3, 'None', 'First Semester', 1, 2018, 'Electrical Engineering'),
(41, 'GE 5', 'Purposive Communication', 3, 'None', 'First Semester', 1, 2018, 'Electrical Engineering'),
(42, 'EM 1', 'Mathematics for Engineers 1', 3, 'None', 'First Semester', 1, 2018, 'Electrical Engineering'),
(43, 'EM 2', 'Calculus 1 (Differential Calculus)', 3, 'None', 'First Semester', 1, 2018, 'Electrical Engineering'),
(44, 'ES1', 'Chemistry for Engineers with Lab', 4, 'None', 'First Semester', 1, 2018, 'Electrical Engineering'),
(45, 'EEO', 'Electrical Engineering Orientation', 2, 'None', 'First Semester', 1, 2018, 'Electrical Engineering'),
(46, 'MST 1', 'Environmental Science', 3, 'None', 'First Semester', 1, 2018, 'Electrical Engineering'),
(47, 'PATHFIT 1', 'Physical Education 1', 2, 'None', 'First Semester', 1, 2018, 'Electrical Engineering'),
(48, 'NSTP 1', 'National Service Training Program', 3, 'None', 'First Semester', 1, 2018, 'Electrical Engineering'),
(49, 'GE 1', 'Understanding the Self', 3, 'None', 'Second Semester', 1, 2018, 'Electrical Engineering'),
(50, 'GE 3', 'Contemporary World', 3, 'None', 'Second Semester', 1, 2018, 'Electrical Engineering'),
(51, 'GE 7', 'Science Technology & Society', 3, 'None', 'Second Semester', 1, 2018, 'Electrical Engineering'),
(52, 'GE 8', 'Ethics', 3, 'None', 'Second Semester', 1, 2018, 'Electrical Engineering'),
(53, 'FIL 2', 'Filipino sa Iba\'t Ibang Disiplina', 3, 'None', 'Second Semester', 1, 2018, 'Electrical Engineering'),
(54, 'EM 4', 'Calculus 2(Integral Calculus)', 3, 'EM 2', 'Second Semester', 1, 2018, 'Electrical Engineering'),
(55, 'EM 3', 'Mathematics for Engineers 2', 3, 'EM 1', 'Second Semester', 1, 2018, 'Electrical Engineering'),
(56, 'ES 2', 'Physics for Engineers with Lab', 4, 'co req EM 4', 'Second Semester', 1, 2018, 'Electrical Engineering'),
(57, 'PE 2', 'Physical Education 2', 2, 'PE 1', 'Second Semester', 1, 2018, 'Electrical Engineering'),
(58, 'NSTP 2', 'National Service Training Program 2', 3, 'NSTP 1', 'Second Semester', 1, 2018, 'Electrical Engineering'),
(59, 'EM 5', 'Engineering Economics', 3, 'EM 1', 'First Semester', 2, 2018, 'Electrical Engineering'),
(60, 'EM 6', 'Differential Equation', 3, 'EM 4', 'First Semester', 2, 2018, 'Electrical Engineering'),
(61, 'EE CP 1', 'Computer Programming', 1, '2nd Year Standing', 'First Semester', 2, 2018, 'Electrical Engineering'),
(62, 'DR 1', 'Engineering Drawing and Plans', 1, '2nd year standing', 'First Semester', 2, 2018, 'Electrical Engineering'),
(63, 'ES 3', 'Engineering Mechanics 1 (Statics)', 3, 'ES 2', 'First Semester', 2, 2018, 'Electrical Engineering'),
(64, 'ES 5', 'Environmental Science and Engineering', 2, 'MST 1 and ES 1', 'First Semester', 2, 2018, 'Electrical Engineering'),
(65, 'EE 11', 'Electrical Circuits with Lab (DC)', 4, 'EM 4 and  ES 2', 'First Semester', 2, 2018, 'Electrical Engineering'),
(66, 'PE 3', 'Physical Education 3', 2, 'PE 2', 'First Semester', 2, 2018, 'Electrical Engineering'),
(67, 'FIL 1', 'Pagsasaling Wika (Teknikal)', 3, 'None', 'First Semester', 2, 2018, 'Electrical Engineering'),
(68, 'MST 4', 'Living the IT Era', 3, 'None', 'First Semester', 2, 2018, 'Electrical Engineering'),
(69, 'GE 6', 'Art Appreciation', 3, 'None', 'First Semester', 2, 2018, 'Electrical Engineering'),
(70, 'DR 2', 'Computer Aided Drafting', 1, 'DR 1', 'Second Semester', 2, 2018, 'Electrical Engineering'),
(71, 'EM 7', 'Engineering Data Analysis', 3, 'EM 3', 'Second Semester', 2, 2018, 'Electrical Engineering'),
(72, 'ES 4', 'Engineering Mechanics 2(Dynamics)', 3, 'ES 3 co req EES 1', 'Second Semester', 2, 2018, 'Electrical Engineering'),
(73, 'EES 1', 'Fundamentals of Deformable Bodies', 2, 'ES 3 co req ES 4', 'Second Semester', 2, 2018, 'Electrical Engineering'),
(74, 'EE 13', 'Electrical Circuits 2 (AC Circuits)', 4, 'EE 11', 'Second Semester', 2, 2018, 'Electrical Engineering'),
(75, 'EES 2', 'Basic Thermodynamics', 2, ' ES 2', 'Second Semester', 2, 2018, 'Electrical Engineering'),
(76, 'RIZ 1', 'Life and Works of Rizal', 3, 'NONE', 'Second Semester', 2, 2018, 'Electrical Engineering'),
(77, 'PE 4', 'Physical Education 4', 2, 'PE 3', 'Second Semester', 2, 2018, 'Electrical Engineering'),
(78, 'AH 4', 'Reading Visual Arts', 3, 'None', 'Second Semester', 2, 2018, 'Electrical Engineering'),
(79, 'EE 12', 'Electronic Circuits: Devices and Analysis', 4, 'EM 4 and EE 11', 'Second Semester', 2, 2018, 'Electrical Engineering'),
(80, 'EEM 1', 'Engineering Mathematics for EE', 3, 'EM 6', 'First Semester', 3, 2018, 'Electrical Engineering'),
(81, 'EE 14', 'Engineering Electromagnetics', 2, 'EM6 and ES 2', 'First Semester', 3, 2018, 'Electrical Engineering'),
(82, 'EE 16', 'Logic Circuits and Switching Theory', 2, 'EE 12', 'First Semester', 3, 2018, 'Electrical Engineering'),
(83, 'ECE 11', 'Fundamentals of Electronic Communications ', 3, 'EE 12', 'First Semester', 3, 2018, 'Electrical Engineering'),
(84, 'EE 15', 'Electrical Machines 1 with LAB', 3, 'EE 13', 'First Semester', 3, 2018, 'Electrical Engineering'),
(85, 'EES 3', 'Fluid Mechanics', 2, 'ES 2', 'First Semester', 3, 2018, 'Electrical Engineering'),
(86, 'EE 17', 'Electrical Apparatus and Devices with Lab', 3, 'EE 11', 'First Semester', 3, 2018, 'Electrical Engineering'),
(87, 'EES 4', 'Material Science and Engineering', 2, 'ES 1', 'First Semester', 3, 2018, 'Electrical Engineering'),
(88, 'BOSH', 'Basic Occupational Safety and Health', 3, 'ES 5', 'First Semester', 3, 2018, 'Electrical Engineering'),
(89, 'EEM 2', 'Numerical Methods and Analysis with Computer Applications', 3, 'EEM 1', 'Second Semester', 3, 2018, 'Electrical Engineering'),
(90, 'EE 18', 'Microprocessor Systems', 3, 'EE 16', 'Second Semester', 3, 2018, 'Electrical Engineering'),
(91, 'TECHNO', 'Technopreneurship', 3, '3rd year standing', 'Second Semester', 3, 2018, 'Electrical Engineering'),
(92, 'EE 19', 'Electrical Machines 2 with LAB', 3, 'EE 15', 'Second Semester', 3, 2018, 'Electrical Engineering'),
(93, 'ECE 12', 'Industrial Electronics with LAB', 4, 'EE 12', 'Second Semester', 3, 2018, 'Electrical Engineering'),
(94, 'EE 20', 'Electrical Standards and Practices', 1, 'EE 20', 'Second Semester', 3, 2018, 'Electrical Engineering'),
(95, 'EE 21', 'Feedback Control Systems', 2, 'EEM 1 and EE12', 'Second Semester', 3, 2018, 'Electrical Engineering'),
(96, 'EE 22', 'Electrical Equipment Operation and Maintenance', 3, 'EE 15', 'Second Semester', 3, 2018, 'Electrical Engineering'),
(97, 'EE 25', 'Research Methods for EE', 1, 'BOSH and EES 1 and ES 5', 'Second Semester', 3, 2018, 'Electrical Engineering'),
(98, 'GE 2', 'Readings in Philippine History', 3, 'None', 'First Semester', 1, 2018, 'Electrical Engineering'),
(99, 'GE 4', 'Mathematics in the Modern World', 3, 'None', 'First Semester', 1, 2018, 'Electrical Engineering'),
(100, 'GE 5', 'Purposive Communication', 3, 'None', 'First Semester', 1, 2018, 'Electrical Engineering'),
(101, 'EM 1', 'Mathematics for Engineers 1', 3, 'None', 'First Semester', 1, 2018, 'Electrical Engineering'),
(102, 'EM 2', 'Calculus 1 (Differential Calculus)', 3, 'None', 'First Semester', 1, 2018, 'Electrical Engineering'),
(103, 'ES1', 'Chemistry for Engineers with Lab', 4, 'None', 'First Semester', 1, 2018, 'Electrical Engineering'),
(104, 'EEO', 'Electrical Engineering Orientation', 2, 'None', 'First Semester', 1, 2018, 'Electrical Engineering'),
(105, 'MST 1', 'Environmental Science', 3, 'None', 'First Semester', 1, 2018, 'Electrical Engineering'),
(106, 'PATHFIT 1', 'Physical Education 1', 2, 'None', 'First Semester', 1, 2018, 'Electrical Engineering'),
(107, 'NSTP 1', 'National Service Training Program', 3, 'None', 'First Semester', 1, 2018, 'Electrical Engineering'),
(108, 'GE 1', 'Understanding the Self', 3, 'None', 'Second Semester', 1, 2018, 'Electrical Engineering'),
(109, 'GE 3', 'Contemporary World', 3, 'None', 'Second Semester', 1, 2018, 'Electrical Engineering'),
(110, 'GE 7', 'Science Technology & Society', 3, 'None', 'Second Semester', 1, 2018, 'Electrical Engineering'),
(111, 'GE 8', 'Ethics', 3, 'None', 'Second Semester', 1, 2018, 'Electrical Engineering'),
(112, 'FIL 2', 'Filipino sa Iba\'t Ibang Disiplina', 3, 'None', 'Second Semester', 1, 2018, 'Electrical Engineering'),
(113, 'EM 4', 'Calculus 2(Integral Calculus)', 3, 'EM 2', 'Second Semester', 1, 2018, 'Electrical Engineering'),
(114, 'EM 3', 'Mathematics for Engineers 2', 3, 'EM 1', 'Second Semester', 1, 2018, 'Electrical Engineering'),
(115, 'ES 2', 'Physics for Engineers with Lab', 4, 'co req EM 4', 'Second Semester', 1, 2018, 'Electrical Engineering'),
(116, 'PE 2', 'Physical Education 2', 2, 'PE 1', 'Second Semester', 1, 2018, 'Electrical Engineering'),
(117, 'NSTP 2', 'National Service Training Program 2', 3, 'NSTP 1', 'Second Semester', 1, 2018, 'Electrical Engineering'),
(118, 'EM 5', 'Engineering Economics', 3, 'EM 1', 'First Semester', 2, 2018, 'Electrical Engineering'),
(119, 'EM 6', 'Differential Equation', 3, 'EM 4', 'First Semester', 2, 2018, 'Electrical Engineering'),
(120, 'EE CP 1', 'Computer Programming', 1, '2nd Year Standing', 'First Semester', 2, 2018, 'Electrical Engineering'),
(121, 'DR 1', 'Engineering Drawing and Plans', 1, '2nd year standing', 'First Semester', 2, 2018, 'Electrical Engineering'),
(122, 'ES 3', 'Engineering Mechanics 1 (Statics)', 3, 'ES 2', 'First Semester', 2, 2018, 'Electrical Engineering'),
(123, 'ES 5', 'Environmental Science and Engineering', 2, 'MST 1 and ES 1', 'First Semester', 2, 2018, 'Electrical Engineering'),
(124, 'EE 11', 'Electrical Circuits with Lab (DC)', 4, 'EM 4 and  ES 2', 'First Semester', 2, 2018, 'Electrical Engineering'),
(125, 'PE 3', 'Physical Education 3', 2, 'PE 2', 'First Semester', 2, 2018, 'Electrical Engineering'),
(126, 'FIL 1', 'Pagsasaling Wika (Teknikal)', 3, 'None', 'First Semester', 2, 2018, 'Electrical Engineering'),
(127, 'MST 4', 'Living the IT Era', 3, 'None', 'First Semester', 2, 2018, 'Electrical Engineering'),
(128, 'GE 6', 'Art Appreciation', 3, 'None', 'First Semester', 2, 2018, 'Electrical Engineering'),
(129, 'DR 2', 'Computer Aided Drafting', 1, 'DR 1', 'Second Semester', 2, 2018, 'Electrical Engineering'),
(130, 'EM 7', 'Engineering Data Analysis', 3, 'EM 3', 'Second Semester', 2, 2018, 'Electrical Engineering'),
(131, 'ES 4', 'Engineering Mechanics 2(Dynamics)', 3, 'ES 3 co req EES 1', 'Second Semester', 2, 2018, 'Electrical Engineering'),
(132, 'EES 1', 'Fundamentals of Deformable Bodies', 2, 'ES 3 co req ES 4', 'Second Semester', 2, 2018, 'Electrical Engineering'),
(133, 'EE 13', 'Electrical Circuits 2 (AC Circuits)', 4, 'EE 11', 'Second Semester', 2, 2018, 'Electrical Engineering'),
(134, 'EES 2', 'Basic Thermodynamics', 2, ' ES 2', 'Second Semester', 2, 2018, 'Electrical Engineering'),
(135, 'RIZ 1', 'Life and Works of Rizal', 3, 'NONE', 'Second Semester', 2, 2018, 'Electrical Engineering'),
(136, 'PE 4', 'Physical Education 4', 2, 'PE 3', 'Second Semester', 2, 2018, 'Electrical Engineering'),
(137, 'AH 4', 'Reading Visual Arts', 3, 'None', 'Second Semester', 2, 2018, 'Electrical Engineering'),
(138, 'EE 12', 'Electronic Circuits: Devices and Analysis', 4, 'EM 4 and EE 11', 'Second Semester', 2, 2018, 'Electrical Engineering'),
(139, 'EEM 1', 'Engineering Mathematics for EE', 3, 'EM 6', 'First Semester', 3, 2018, 'Electrical Engineering'),
(140, 'EE 14', 'Engineering Electromagnetics', 2, 'EM6 and ES 2', 'First Semester', 3, 2018, 'Electrical Engineering'),
(141, 'EE 16', 'Logic Circuits and Switching Theory', 2, 'EE 12', 'First Semester', 3, 2018, 'Electrical Engineering'),
(142, 'ECE 11', 'Fundamentals of Electronic Communications ', 3, 'EE 12', 'First Semester', 3, 2018, 'Electrical Engineering'),
(143, 'EE 15', 'Electrical Machines 1 with LAB', 3, 'EE 13', 'First Semester', 3, 2018, 'Electrical Engineering'),
(144, 'EES 3', 'Fluid Mechanics', 2, 'ES 2', 'First Semester', 3, 2018, 'Electrical Engineering'),
(145, 'EE 17', 'Electrical Apparatus and Devices with Lab', 3, 'EE 11', 'First Semester', 3, 2018, 'Electrical Engineering'),
(146, 'EES 4', 'Material Science and Engineering', 2, 'ES 1', 'First Semester', 3, 2018, 'Electrical Engineering'),
(147, 'BOSH', 'Basic Occupational Safety and Health', 3, 'ES 5', 'First Semester', 3, 2018, 'Electrical Engineering'),
(148, 'EEM 2', 'Numerical Methods and Analysis with Computer Applications', 3, 'EEM 1', 'Second Semester', 3, 2018, 'Electrical Engineering'),
(149, 'EE 18', 'Microprocessor Systems', 3, 'EE 16', 'Second Semester', 3, 2018, 'Electrical Engineering'),
(150, 'TECHNO', 'Technopreneurship', 3, '3rd year standing', 'Second Semester', 3, 2018, 'Electrical Engineering'),
(151, 'EE 19', 'Electrical Machines 2 with LAB', 3, 'EE 15', 'Second Semester', 3, 2018, 'Electrical Engineering'),
(152, 'ECE 12', 'Industrial Electronics with LAB', 4, 'EE 12', 'Second Semester', 3, 2018, 'Electrical Engineering'),
(153, 'EE 20', 'Electrical Standards and Practices', 1, 'EE 20', 'Second Semester', 3, 2018, 'Electrical Engineering'),
(154, 'EE 21', 'Feedback Control Systems', 2, 'EEM 1 and EE12', 'Second Semester', 3, 2018, 'Electrical Engineering'),
(155, 'EE 22', 'Electrical Equipment Operation and Maintenance', 3, 'EE 15', 'Second Semester', 3, 2018, 'Electrical Engineering'),
(156, 'EE 25', 'Research Methods for EE', 1, 'BOSH and EES 1 and ES 5', 'Second Semester', 3, 2018, 'Electrical Engineering'),
(157, 'EE OJT', 'On the Job Training (240 hours)', 2, ' ', 'Summer', 0, 2018, 'Electrical Engineering'),
(158, 'EE 23', 'EE Laws Code and Professional Ethics', 2, 'EE20', 'First Semester', 4, 2018, 'Electrical Engineering'),
(159, 'EE 24', 'Electrical System and Illumination Engineering Design with LAB', 5, '4th year standing', 'First Semester', 4, 2018, 'Electrical Engineering'),
(160, 'EE ELEC 1', 'EE Elective 1 (Power System Protection )', 3, '4th year standing', 'First Semester', 4, 2018, 'Electrical Engineering'),
(161, 'ES 6', ' Management of Engineering Projects', 2, '4th year standing', 'First Semester', 4, 2018, 'Electrical Engineering'),
(162, 'EE 26', 'Distribution Systems and Substation Design', 3, 'EE 22', 'First Semester', 4, 2018, 'Electrical Engineering'),
(163, 'ST 1', 'Special Topics(Mathematics and ESAS)', 1, '4th year standing', 'First Semester', 4, 2018, 'Electrical Engineering'),
(164, 'EE 27', 'Electrical Transients with LAB', 4, 'EE 13 and EM 6', 'First Semester', 4, 2018, 'Electrical Engineering'),
(165, 'EE TH1', 'Thesis 1', 1, 'EE 25', 'First Semester', 4, 2018, 'Electrical Engineering'),
(166, 'EE 29', 'Power Systems and Analysis with Lab', 4, 'EE 19 and EE 23', 'Second Semester', 4, 2018, 'Electrical Engineering'),
(167, 'EE ELEC 2', 'EE Elective 2 (Renewable Energy)', 3, 'EE ELEC 1', 'Second Semester', 4, 2018, 'Electrical Engineering'),
(168, 'EE 28', 'Instrumentation and Control with Lab', 3, 'EE 21', 'Second Semester', 4, 2018, 'Electrical Engineering'),
(169, 'EE 30', 'Research Project or Capstone Design Project(Thesis 2)', 1, 'EE 25', 'Second Semester', 4, 2018, 'Electrical Engineering'),
(170, 'EE 31', 'Seminars/ Colloquia', 1, '4th year standing', 'Second Semester', 4, 2018, 'Electrical Engineering'),
(171, 'EE 32', 'Fundamentals of Power Plant Engineering and Design', 1, 'EE 19 and EE 23', 'Second Semester', 4, 2018, 'Electrical Engineering'),
(172, 'ST 2', 'Special Topics 2 (EE Review)', 1, 'ST1', 'Second Semester', 4, 2018, 'Electrical Engineering');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_grade_record`
--

CREATE TABLE `tbl_grade_record` (
  `record_ID` int(11) NOT NULL,
  `record_StudentID` int(11) DEFAULT NULL,
  `record_FinalGrade` double DEFAULT NULL,
  `record_Remarks` varchar(50) DEFAULT NULL,
  `record_CourseID` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- --------------------------------------------------------

--
-- Table structure for table `tbl_student`
--

CREATE TABLE `tbl_student` (
  `student_ID` int(11) NOT NULL,
  `student_StudentNo` varchar(50) NOT NULL DEFAULT 'NULL',
  `student_Name` varchar(50) DEFAULT NULL,
  `student_Department` varchar(50) DEFAULT NULL,
  `student_Batch` int(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_student`
--

INSERT INTO `tbl_student` (`student_ID`, `student_StudentNo`, `student_Name`, `student_Department`, `student_Batch`) VALUES
(1, '18-0159', 'Student1', 'Information Technology', 2018);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `tbl_account`
--
ALTER TABLE `tbl_account`
  ADD PRIMARY KEY (`account_ID`);

--
-- Indexes for table `tbl_curriculum`
--
ALTER TABLE `tbl_curriculum`
  ADD PRIMARY KEY (`curr_ID`);

--
-- Indexes for table `tbl_grade_record`
--
ALTER TABLE `tbl_grade_record`
  ADD PRIMARY KEY (`record_ID`),
  ADD KEY `report_CourseID` (`record_CourseID`),
  ADD KEY `report_StudentID` (`record_StudentID`);

--
-- Indexes for table `tbl_student`
--
ALTER TABLE `tbl_student`
  ADD PRIMARY KEY (`student_ID`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `tbl_curriculum`
--
ALTER TABLE `tbl_curriculum`
  MODIFY `curr_ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=173;

--
-- AUTO_INCREMENT for table `tbl_student`
--
ALTER TABLE `tbl_student`
  MODIFY `student_ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `tbl_grade_record`
--
ALTER TABLE `tbl_grade_record`
  ADD CONSTRAINT `tbl_grade_record_ibfk_1` FOREIGN KEY (`record_CourseID`) REFERENCES `tbl_curriculum` (`curr_ID`),
  ADD CONSTRAINT `tbl_grade_record_ibfk_2` FOREIGN KEY (`record_StudentID`) REFERENCES `tbl_student` (`student_ID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
