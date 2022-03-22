-- phpMyAdmin SQL Dump
-- version 4.9.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Mar 22, 2022 at 03:10 PM
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
(1, 'admin1', 'admin1@gmail.com', 'admin1', 1),
(2, 'admin2', 'admin2@gmail.com', 'admin2', 2),
(3, 'admin3', 'admin3@gmail.com', 'admin3', 3);

-- --------------------------------------------------------

--
-- Table structure for table `tbl_curriculum`
--

CREATE TABLE `tbl_curriculum` (
  `curr_ID` int(11) NOT NULL,
  `curr_No` int(10) NOT NULL,
  `curr_Code` varchar(50) DEFAULT NULL,
  `curr_Title` varchar(100) DEFAULT NULL,
  `curr_Units` int(11) DEFAULT NULL,
  `curr_Pre_Req` varchar(20) DEFAULT NULL,
  `curr_Department` int(11) DEFAULT NULL,
  `curr_Yearlevel` int(10) NOT NULL,
  `curr_Semester` smallint(2) NOT NULL,
  `curr_Batch` int(10) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data for table `tbl_curriculum`
--

INSERT INTO `tbl_curriculum` (`curr_ID`, `curr_No`, `curr_Code`, `curr_Title`, `curr_Units`, `curr_Pre_Req`, `curr_Department`, `curr_Yearlevel`, `curr_Semester`, `curr_Batch`) VALUES
(1, 1, 'GE 2', 'Reading in Philippine History ', 3, '', 3, 1, 0, 2018),
(2, 2, 'GE 4', 'Mathematics in the Modern World ', 3, '', 3, 1, 0, 2018),
(3, 3, 'GE 5', 'Purposive Communication ', 3, '', 3, 1, 0, 2018),
(4, 4, 'GE 6', 'Arts Appreciation', 3, '', 3, 1, 0, 2018),
(5, 5, 'CC 1', 'Introduction to Computing ', 2, '', 3, 1, 0, 2018),
(6, 6, 'CC 1L', 'Introduction to Computing (Lab)', 1, '', 3, 1, 0, 2018),
(7, 7, 'CC 2', 'Computer Programming', 2, '', 3, 1, 0, 2018),
(8, 8, 'CC 2L', 'Computer Programming (Lab)', 1, '', 3, 1, 0, 2018),
(9, 9, 'HCI 101', 'Introduction to Human Computer Interaction', 3, '', 3, 1, 0, 2018),
(10, 10, 'MST 1', 'Environmental Science', 3, '', 3, 1, 0, 2018),
(11, 11, 'PATH Fit 1', 'Physical Fitness and Related Activities', 2, '', 3, 1, 0, 2018),
(12, 12, 'NSTP 1', 'National Service and Training Program', 3, '', 3, 1, 0, 2018),
(13, 1, 'GE 1', 'Understanding the Self', 3, '', 3, 1, 1, 2018),
(14, 2, 'GE 3', 'Contemporary World', 3, '', 3, 1, 1, 2018),
(15, 3, 'GE 7', 'Science, Technology, & Society', 3, '', 3, 1, 1, 2018),
(16, 4, 'GE 8', 'Ethics', 3, '', 3, 1, 1, 2018),
(17, 5, 'CC 3', 'Computer Programming 2', 2, 'CC 2', 3, 1, 1, 2018),
(18, 6, 'CC 3L', 'Computer Programming 2 (Lab)', 1, 'CC 2L', 3, 1, 1, 2018),
(19, 7, 'MS 101', 'Discrete Mathematics', 3, '', 3, 1, 1, 2018),
(20, 8, 'AH 4', 'Reading Visual Arts', 3, '', 3, 1, 1, 2018),
(21, 9, 'PATH FIT 2', 'Physical Fitness and Related Activities', 2, 'PATH FIT 1', 3, 1, 1, 2018),
(22, 10, 'NSTP 2', 'National Service and Training Program', 3, 'NSTP 1', 3, 1, 1, 2018),
(23, 11, 'LIT 1', 'Panitikan ng Pilipinas', 2, '', 3, 1, 1, 2018);

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
(2, 'BSEE', 'Bachelor of Science in Electrical Engineering', 'Jemuel D. Almerol'),
(3, 'BSIT', 'Bachelor of Science in Information Technology', 'Patrick Luis Francisco');

-- --------------------------------------------------------

--
-- Table structure for table `tbl_report`
--

CREATE TABLE `tbl_report` (
  `report_ID` int(11) NOT NULL,
  `report_Course` int(11) DEFAULT NULL,
  `report_Batch` int(10) DEFAULT NULL,
  `report_Department` int(11) DEFAULT NULL,
  `report_StudentID` int(11) DEFAULT NULL,
  `report_StudentName` varchar(100) DEFAULT NULL,
  `report_StudentYear` int(10) DEFAULT NULL,
  `report_Grade` double DEFAULT NULL,
  `report_Remarks` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

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
  ADD KEY `course_Department` (`curr_Department`);

--
-- Indexes for table `tbl_department`
--
ALTER TABLE `tbl_department`
  ADD PRIMARY KEY (`dept_ID`);

--
-- Indexes for table `tbl_report`
--
ALTER TABLE `tbl_report`
  ADD PRIMARY KEY (`report_ID`),
  ADD KEY `report_Course` (`report_Course`),
  ADD KEY `report_Department` (`report_Department`),
  ADD KEY `report_StudentID` (`report_StudentID`);

--
-- Indexes for table `tbl_student`
--
ALTER TABLE `tbl_student`
  ADD PRIMARY KEY (`student_ID`),
  ADD KEY `student_Department` (`student_Department`);

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
-- Constraints for table `tbl_report`
--
ALTER TABLE `tbl_report`
  ADD CONSTRAINT `tbl_report_ibfk_1` FOREIGN KEY (`report_Course`) REFERENCES `tbl_curriculum` (`curr_ID`),
  ADD CONSTRAINT `tbl_report_ibfk_2` FOREIGN KEY (`report_Department`) REFERENCES `tbl_department` (`dept_ID`),
  ADD CONSTRAINT `tbl_report_ibfk_3` FOREIGN KEY (`report_StudentID`) REFERENCES `tbl_student` (`student_ID`);

--
-- Constraints for table `tbl_student`
--
ALTER TABLE `tbl_student`
  ADD CONSTRAINT `tbl_student_ibfk_1` FOREIGN KEY (`student_Department`) REFERENCES `tbl_department` (`dept_ID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
