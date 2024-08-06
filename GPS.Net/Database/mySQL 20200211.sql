-- MySQL dump 10.13  Distrib 8.0.18, for Win64 (x86_64)
--
-- Host: 127.0.0.1    Database: greatechapp
-- ------------------------------------------------------
-- Server version	8.0.18

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `tbl_accesscontrol_list`
--

DROP TABLE IF EXISTS `tbl_accesscontrol_list`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_accesscontrol_list` (
  `UserLevel_ID` int(11) NOT NULL AUTO_INCREMENT,
  `UserLevel` varchar(45) DEFAULT NULL,
  `About` int(1) DEFAULT NULL,
  `Communication` int(1) DEFAULT NULL,
  `IO` int(1) DEFAULT NULL,
  `LogHistory` int(1) DEFAULT NULL,
  `UserMgmt` int(1) DEFAULT NULL,
  `GalilController` int(1) DEFAULT NULL,
  `TeachPoint` int(1) DEFAULT NULL,
  `ACSController` int(1) DEFAULT NULL,
  PRIMARY KEY (`UserLevel_ID`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_accesscontrol_list`
--

LOCK TABLES `tbl_accesscontrol_list` WRITE;
/*!40000 ALTER TABLE `tbl_accesscontrol_list` DISABLE KEYS */;
INSERT INTO `tbl_accesscontrol_list` VALUES (1,'Admin',1,1,1,1,1,1,1,1),(2,'Engineer',1,1,1,1,1,1,1,1),(3,'Technician',1,1,1,1,0,1,1,1),(4,'Operator',1,0,1,1,0,1,1,1);
/*!40000 ALTER TABLE `tbl_accesscontrol_list` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `tbl_user_list`
--

DROP TABLE IF EXISTS `tbl_user_list`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tbl_user_list` (
  `User_ID` int(11) NOT NULL AUTO_INCREMENT,
  `Username` varchar(45) DEFAULT NULL,
  `UserPassword` varchar(45) DEFAULT NULL,
  `UserLevel` varchar(45) DEFAULT NULL,
  `DisplayName` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`User_ID`)
) ENGINE=InnoDB AUTO_INCREMENT=11 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `tbl_user_list`
--

LOCK TABLES `tbl_user_list` WRITE;
/*!40000 ALTER TABLE `tbl_user_list` DISABLE KEYS */;
INSERT INTO `tbl_user_list` VALUES (1,'a','a','Admin','Mohd Ali'),(2,'b','b','Engineer','Safar');
/*!40000 ALTER TABLE `tbl_user_list` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2020-02-11  9:33:39
