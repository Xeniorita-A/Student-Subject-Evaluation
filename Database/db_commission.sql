-- phpMyAdmin SQL Dump
-- version 4.9.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Apr 27, 2022 at 06:52 PM
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
  `account_Department` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_account`
--

INSERT INTO `tbl_account` (`account_ID`, `account_Username`, `account_Email`, `account_Password`, `account_Department`) VALUES
(1, 'admin1', 'admin1@gmail.com', '25f43b1486ad95a1398e3eeb3d83bc4010015fcc9bedb35b432e00298d5021f7', 1),
(2, 'admin2', 'Almerol@gmail.com', '1c142b2d01aa34e9a36bde480645a57fd69e14155dacfab5a3f9257b77fdc8d8', 2),
(3, 'admin', 'admin@gmail.com', '8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918', 3);

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
  `curr_Semester` varchar(50) DEFAULT NULL,
  `curr_Yearlevel` int(11) NOT NULL,
  `curr_Batch` int(11) DEFAULT NULL,
  `curr_Department` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_curriculum`
--

INSERT INTO `tbl_curriculum` (`curr_ID`, `curr_Code`, `curr_Title`, `curr_Units`, `curr_Pre_Req`, `curr_Semester`, `curr_Yearlevel`, `curr_Batch`, `curr_Department`) VALUES
(1, 'GE 5', 'Purposive Communication', 3, 'None', 'First Sem ', 1, 2018, 1),
(2, 'MST 1', 'Environmental Science', 3, 'None', 'First Sem ', 1, 2018, 1),
(3, 'GE 4', 'Mathematics in Modern World', 3, 'None', 'First Sem ', 1, 2018, 1),
(4, 'GE 2', 'Readings in Philippine History', 3, 'None', 'First Sem ', 1, 2018, 1),
(5, 'EM 1', 'Mathematics for Engineers 1', 3, 'None', 'First Sem ', 1, 2018, 1),
(6, 'EM 2', 'Calculus 1 (Differential Calculus', 3, 'None', 'First Sem ', 1, 2018, 1),
(7, 'ES 1', 'Chemistry for Engineers with Lab', 4, 'None', 'First Sem ', 1, 2018, 1),
(8, 'CE 1', 'CE Orientation', 2, 'None', 'First Sem ', 1, 2018, 1),
(9, 'PE 1', 'Physical Education 1', 2, 'None', 'First Sem ', 1, 2018, 1),
(10, 'NSTP 1', 'National Service Training Program', 3, 'None', 'First Sem ', 1, 2018, 1),
(11, 'GE 3', 'Contemporary World', 3, 'None', 'Second Sem', 1, 2018, 1),
(12, 'GE 7', 'Science Technology & Society', 3, 'None', 'Second Sem', 1, 2018, 1),
(13, 'GE 1', 'Understanding the Self', 3, 'None', 'Second Sem', 1, 2018, 1),
(14, 'GE 8', 'Ethics', 3, 'None', 'Second Sem', 1, 2018, 1),
(15, 'EM 4', 'Calculus 2 (Integral Calculus)', 3, 'EM2', 'Second Sem', 1, 2018, 1),
(16, 'ES 2', 'Physics for Engineers with Lab (Calculus Based)', 4, 'EM2 co req EM3', 'Second Sem', 1, 2018, 1),
(17, 'EM 3', 'Mathematics for Engineers 2', 3, 'EM 1', 'Second Sem', 1, 2018, 1),
(18, 'PE 2', 'Physical Education', 2, 'PE1', 'Second Sem', 1, 2018, 1),
(19, 'NSTP 2', 'National Service Training Program ', 3, 'NSTP 1', 'Second Sem', 1, 2018, 1),
(20, 'LIT 1', 'Panitikang Panlipunan', 3, 'None', 'Second Sem', 1, 2018, 1),
(21, 'CP1', 'Computer Fundamentals and Programming', 2, '2nd year standing', 'First Sem ', 2, 2018, 1),
(22, 'EM6', 'Differential Equation', 3, 'EM4', 'First Sem ', 2, 2018, 1),
(23, 'ES3', 'Statics of Rigid Bodies', 3, 'EM4 ES2', 'First Sem ', 2, 2018, 1),
(24, 'DR1', 'Engineering Drawing and Plans', 1, '2nd year standing', 'First Sem ', 2, 2018, 1),
(25, 'CE2', 'Fundamentals of Surveying with Lab', 4, 'co req DR1 EM1', 'First Sem ', 2, 2018, 1),
(26, 'FILIPINO 1', 'Pagsasaling Wika (Teknikal)', 3, 'None', 'First Sem ', 2, 2018, 1),
(27, 'M5', 'Engineering Econimoics', 3, 'EM1', 'First Sem ', 2, 2018, 1),
(28, 'MST4', 'Living in an IT Era', 3, 'None', 'First Sem ', 2, 2018, 1),
(29, 'GE6', 'Arts Appreciation', 3, 'None', 'First Sem ', 2, 2018, 1),
(30, 'PE3', 'Physical Education', 2, 'PE 2', 'First Sem ', 2, 2018, 1),
(31, 'DR2', 'Computer Aided Drafting', 1, 'DR 1', 'Second Sem', 2, 2018, 1),
(32, 'EM7', 'Engineering Data Analysis', 3, 'EM6', 'Second Sem', 2, 2018, 1),
(33, 'ES4', 'Dynamics of Rigid Bodies', 3, 'ES3', 'Second Sem', 2, 2018, 1),
(34, 'ES5', 'Mechanics of Deformable Bodies', 4, 'ES3', 'Second Sem', 2, 2018, 1),
(35, 'CE3', 'Building Systems Design with Lab', 3, 'co req DR2', 'Second Sem', 2, 2018, 1),
(36, 'RIZ1', 'Life & Works of Rizal', 3, 'None', 'Second Sem', 2, 2018, 1),
(37, 'CE4', 'Geology for Civil Engineers', 2, 'ES1 2nd year standing', 'Second Sem', 2, 2018, 1),
(38, 'AH4', 'Reading Visual Art', 3, 'None', 'Second Sem', 2, 2018, 1),
(39, 'PE4', 'Physical Education', 2, 'PE 3', 'Second Sem', 2, 2018, 1),
(40, 'FIL2', 'Filipino sa iba\'t ibang disiplina', 3, 'None', 'Second Sem', 2, 2018, 1),
(41, 'CE5', 'Structural Theory with Lab', 4, 'ES5', 'First Sem ', 3, 2018, 1),
(42, 'CE6', 'Highway and Railroad Engineering', 3, 'CE2', 'First Sem ', 3, 2018, 1),
(43, 'AC1', 'Basic Electrical Engineering', 3, 'ES2 3rd year standing', 'First Sem ', 3, 2018, 1),
(44, 'AC2', 'Basic Mechanical Engineering', 3, 'ES2 3rd year standing', 'First Sem ', 3, 2018, 1),
(45, 'EM8', 'Numerical Solutions to CE Problems', 3, 'EM7', 'First Sem ', 3, 2018, 1),
(46, 'CE7', 'Fluid Mechanics with Lab', 3, 'ES4 ES5', 'First Sem ', 3, 2018, 1),
(47, 'CE8', 'Construction Materials and Testing with Lab', 3, 'ES5', 'First Sem ', 3, 2018, 1),
(48, 'CE9', 'Engineering Management', 2, '3rd year standing', 'First Sem ', 3, 2018, 1),
(49, 'TECHNO', 'Technopreneurship 101', 3, '3rd year standing', 'Second Sem', 3, 2018, 1),
(50, 'CE11', 'Principles of Steel Design', 3, 'CE5', 'Second Sem', 3, 2018, 1),
(51, 'CE12', 'Principles of Reinforced Concrete/ Prestressed Concrete', 4, 'CE5', 'Second Sem', 3, 2018, 1),
(52, 'CE13', 'Hydraulics with Lab', 5, 'CE7', 'Second Sem', 3, 2018, 1),
(53, 'CE14', 'Geotechnical Engineering (Soil Mechanics) with Lab', 4, 'ES5 ES4', 'Second Sem', 3, 2018, 1),
(54, 'CE15', 'Quantity Surveying', 1, 'CE3 CE9', 'Second Sem', 3, 2018, 1),
(55, 'CE16', 'Construction Methods and Project Management', 3, 'co-req CE15', 'Second Sem', 3, 2018, 1),
(56, 'CEOJT', 'CE On the Job Training for CE 320 Hours', 3, '3rd year standing', 'Mid year', 0, 2018, 1),
(57, 'CE 17', 'Principles of Transportaion Engineering', 3, 'CE6', 'First Sem ', 4, 2018, 1),
(58, 'CE 18', 'CE Project 1 with Lab', 2, '4th year standing', 'First Sem ', 4, 2018, 1),
(59, 'CE 19', 'Hydrology', 2, 'CE13', 'First Sem ', 4, 2018, 1),
(60, 'CE 20', 'Earthquake Engineering Elective 1', 3, 'CE5', 'First Sem ', 4, 2018, 1),
(61, 'CE SC', 'CE Seminars and Colloquia', 1, 'co req CE18', 'First Sem ', 4, 2018, 1),
(62, 'CE 21', 'CE Laws Ethicsand Contracts', 2, 'CE16', 'First Sem ', 4, 2018, 1),
(63, 'CE 22', 'Foundation and Retaining Wall Design - Elective 2', 3, 'CE12CE14', 'First Sem ', 4, 2018, 1),
(64, 'CE 23', 'CE Integration Course 1', 1, 'CE13CE14EM 8', 'First Sem ', 4, 2018, 1),
(65, 'CE 24', 'CE Project 2', 2, 'CE18', 'Second Sem', 4, 2018, 1),
(66, 'CE 25', 'Computer Software in Structural Analysis - Elective 3', 3, 'CE11 CE12', 'Second Sem', 4, 2018, 1),
(67, 'CE 26', 'Prestressed Concrete Design - Elective 4', 3, 'CE12', 'Second Sem', 4, 2018, 1),
(68, 'CE 27', 'CE Elective 5*', 3, '4th year standing', 'Second Sem', 4, 2018, 1),
(69, 'CE 28', 'CE Elective 6*', 3, '4th year standing', 'Second Sem', 4, 2018, 1),
(70, 'CE 29', 'CE Integration Course 2', 1, 'CE 23', 'Second Sem', 4, 2018, 1);

-- --------------------------------------------------------

--
-- Table structure for table `tbl_department`
--

CREATE TABLE `tbl_department` (
  `dept_ID` int(11) NOT NULL,
  `dept_Code` varchar(20) DEFAULT NULL,
  `dept_Name` varchar(50) DEFAULT NULL,
  `dept_Chairperson` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_department`
--

INSERT INTO `tbl_department` (`dept_ID`, `dept_Code`, `dept_Name`, `dept_Chairperson`) VALUES
(1, 'BSCE', 'Bachelor of Science in Civil Engineering', 'John Lemar Tirao'),
(2, 'BSEE', 'Bachelor of Science in Electrical Engineering', 'Jemuel Almerol'),
(3, 'BSIT', 'Bachelor of Science in Information Technology', 'Patrick Francisco');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_student`
--

CREATE TABLE `tbl_student` (
  `student_ID` int(11) NOT NULL,
  `student_FirstName` varchar(50) DEFAULT NULL,
  `student_MiddleName` varchar(50) DEFAULT NULL,
  `student_LastName` varchar(50) DEFAULT NULL,
  `student_Suffix` varchar(10) DEFAULT NULL,
  `student_YearLevel` int(10) DEFAULT NULL,
  `student_Department` int(11) DEFAULT NULL,
  `student_Batch` varchar(20) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `tbl_account`
--
ALTER TABLE `tbl_account`
  ADD PRIMARY KEY (`account_ID`),
  ADD KEY `account_Department` (`account_Department`);

--
-- Indexes for table `tbl_curriculum`
--
ALTER TABLE `tbl_curriculum`
  ADD PRIMARY KEY (`curr_ID`),
  ADD KEY `curr_Department` (`curr_Department`);

--
-- Indexes for table `tbl_department`
--
ALTER TABLE `tbl_department`
  ADD PRIMARY KEY (`dept_ID`);

--
-- Indexes for table `tbl_student`
--
ALTER TABLE `tbl_student`
  ADD PRIMARY KEY (`student_ID`),
  ADD KEY `student_Department` (`student_Department`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `tbl_curriculum`
--
ALTER TABLE `tbl_curriculum`
  MODIFY `curr_ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=71;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `tbl_account`
--
ALTER TABLE `tbl_account`
  ADD CONSTRAINT `tbl_account_ibfk_1` FOREIGN KEY (`account_Department`) REFERENCES `tbl_department` (`dept_ID`);

--
-- Constraints for table `tbl_curriculum`
--
ALTER TABLE `tbl_curriculum`
  ADD CONSTRAINT `tbl_curriculum_ibfk_1` FOREIGN KEY (`curr_Department`) REFERENCES `tbl_department` (`dept_ID`);

--
-- Constraints for table `tbl_student`
--
ALTER TABLE `tbl_student`
  ADD CONSTRAINT `tbl_student_ibfk_1` FOREIGN KEY (`student_Department`) REFERENCES `tbl_department` (`dept_ID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
