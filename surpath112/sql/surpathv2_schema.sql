-- MySQL dump 10.13  Distrib 8.0.28, for Win64 (x86_64)
--
-- Host: localhost    Database: surpathv2
-- ------------------------------------------------------
-- Server version	8.0.28

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `__efmigrationshistory`
--

DROP TABLE IF EXISTS `__efmigrationshistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abpauditlogs`
--

DROP TABLE IF EXISTS `abpauditlogs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abpauditlogs` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `TenantId` int DEFAULT NULL,
  `UserId` bigint DEFAULT NULL,
  `ServiceName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `MethodName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Parameters` varchar(1024) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ReturnValue` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ExecutionTime` datetime(6) NOT NULL,
  `ExecutionDuration` int NOT NULL,
  `ClientIpAddress` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ClientName` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `BrowserInfo` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ExceptionMessage` varchar(1024) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Exception` varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ImpersonatorUserId` bigint DEFAULT NULL,
  `ImpersonatorTenantId` int DEFAULT NULL,
  `CustomData` varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AbpAuditLogs_TenantId_ExecutionDuration` (`TenantId`,`ExecutionDuration`),
  KEY `IX_AbpAuditLogs_TenantId_ExecutionTime` (`TenantId`,`ExecutionTime`),
  KEY `IX_AbpAuditLogs_TenantId_UserId` (`TenantId`,`UserId`)
) ENGINE=InnoDB AUTO_INCREMENT=13971645 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abpbackgroundjobs`
--

DROP TABLE IF EXISTS `abpbackgroundjobs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abpbackgroundjobs` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `JobType` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `JobArgs` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `TryCount` smallint NOT NULL,
  `NextTryTime` datetime(6) NOT NULL,
  `LastTryTime` datetime(6) DEFAULT NULL,
  `IsAbandoned` tinyint(1) NOT NULL,
  `Priority` tinyint unsigned NOT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AbpBackgroundJobs_IsAbandoned_NextTryTime` (`IsAbandoned`,`NextTryTime`)
) ENGINE=InnoDB AUTO_INCREMENT=19770 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abpdynamicentityproperties`
--

DROP TABLE IF EXISTS `abpdynamicentityproperties`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abpdynamicentityproperties` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `EntityFullName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `DynamicPropertyId` int NOT NULL,
  `TenantId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_AbpDynamicEntityProperties_EntityFullName_DynamicPropertyId_~` (`EntityFullName`,`DynamicPropertyId`,`TenantId`),
  KEY `IX_AbpDynamicEntityProperties_DynamicPropertyId` (`DynamicPropertyId`),
  CONSTRAINT `FK_AbpDynamicEntityProperties_AbpDynamicProperties_DynamicPrope~` FOREIGN KEY (`DynamicPropertyId`) REFERENCES `abpdynamicproperties` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abpdynamicentitypropertyvalues`
--

DROP TABLE IF EXISTS `abpdynamicentitypropertyvalues`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abpdynamicentitypropertyvalues` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Value` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `EntityId` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `DynamicEntityPropertyId` int NOT NULL,
  `TenantId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AbpDynamicEntityPropertyValues_DynamicEntityPropertyId` (`DynamicEntityPropertyId`),
  CONSTRAINT `FK_AbpDynamicEntityPropertyValues_AbpDynamicEntityProperties_Dy~` FOREIGN KEY (`DynamicEntityPropertyId`) REFERENCES `abpdynamicentityproperties` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abpdynamicproperties`
--

DROP TABLE IF EXISTS `abpdynamicproperties`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abpdynamicproperties` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `PropertyName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `DisplayName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `InputType` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Permission` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `TenantId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_AbpDynamicProperties_PropertyName_TenantId` (`PropertyName`,`TenantId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abpdynamicpropertyvalues`
--

DROP TABLE IF EXISTS `abpdynamicpropertyvalues`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abpdynamicpropertyvalues` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Value` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `DynamicPropertyId` int NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AbpDynamicPropertyValues_DynamicPropertyId` (`DynamicPropertyId`),
  CONSTRAINT `FK_AbpDynamicPropertyValues_AbpDynamicProperties_DynamicPropert~` FOREIGN KEY (`DynamicPropertyId`) REFERENCES `abpdynamicproperties` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abpeditions`
--

DROP TABLE IF EXISTS `abpeditions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abpeditions` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Name` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `DisplayName` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Discriminator` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ExpiringEditionId` int DEFAULT NULL,
  `DailyPrice` decimal(65,30) DEFAULT NULL,
  `WeeklyPrice` decimal(65,30) DEFAULT NULL,
  `MonthlyPrice` decimal(65,30) DEFAULT NULL,
  `AnnualPrice` decimal(65,30) DEFAULT NULL,
  `TrialDayCount` int DEFAULT NULL,
  `WaitingDayAfterExpire` int DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abpentitychanges`
--

DROP TABLE IF EXISTS `abpentitychanges`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abpentitychanges` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ChangeTime` datetime(6) NOT NULL,
  `ChangeType` tinyint unsigned NOT NULL,
  `EntityChangeSetId` bigint NOT NULL,
  `EntityId` varchar(48) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `EntityTypeFullName` varchar(192) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `TenantId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AbpEntityChanges_EntityChangeSetId` (`EntityChangeSetId`),
  KEY `IX_AbpEntityChanges_EntityTypeFullName_EntityId` (`EntityTypeFullName`,`EntityId`),
  CONSTRAINT `FK_AbpEntityChanges_AbpEntityChangeSets_EntityChangeSetId` FOREIGN KEY (`EntityChangeSetId`) REFERENCES `abpentitychangesets` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=954537 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abpentitychangesets`
--

DROP TABLE IF EXISTS `abpentitychangesets`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abpentitychangesets` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `BrowserInfo` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ClientIpAddress` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ClientName` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `ExtensionData` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ImpersonatorTenantId` int DEFAULT NULL,
  `ImpersonatorUserId` bigint DEFAULT NULL,
  `Reason` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `TenantId` int DEFAULT NULL,
  `UserId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AbpEntityChangeSets_TenantId_CreationTime` (`TenantId`,`CreationTime`),
  KEY `IX_AbpEntityChangeSets_TenantId_Reason` (`TenantId`,`Reason`),
  KEY `IX_AbpEntityChangeSets_TenantId_UserId` (`TenantId`,`UserId`)
) ENGINE=InnoDB AUTO_INCREMENT=740754 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abpentitypropertychanges`
--

DROP TABLE IF EXISTS `abpentitypropertychanges`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abpentitypropertychanges` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `EntityChangeId` bigint NOT NULL,
  `NewValue` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `OriginalValue` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `PropertyName` varchar(96) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `PropertyTypeFullName` varchar(192) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `TenantId` int DEFAULT NULL,
  `NewValueHash` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `OriginalValueHash` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  KEY `IX_AbpEntityPropertyChanges_EntityChangeId` (`EntityChangeId`),
  CONSTRAINT `FK_AbpEntityPropertyChanges_AbpEntityChanges_EntityChangeId` FOREIGN KEY (`EntityChangeId`) REFERENCES `abpentitychanges` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=8405719 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abpfeatures`
--

DROP TABLE IF EXISTS `abpfeatures`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abpfeatures` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `TenantId` int DEFAULT NULL,
  `Name` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Value` varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Discriminator` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `EditionId` int DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AbpFeatures_EditionId_Name` (`EditionId`,`Name`),
  KEY `IX_AbpFeatures_TenantId_Name` (`TenantId`,`Name`),
  CONSTRAINT `FK_AbpFeatures_AbpEditions_EditionId` FOREIGN KEY (`EditionId`) REFERENCES `abpeditions` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=42 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abplanguages`
--

DROP TABLE IF EXISTS `abplanguages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abplanguages` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `TenantId` int DEFAULT NULL,
  `Name` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `DisplayName` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Icon` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `IsDisabled` tinyint(1) NOT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AbpLanguages_TenantId_Name` (`TenantId`,`Name`)
) ENGINE=InnoDB AUTO_INCREMENT=16 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abplanguagetexts`
--

DROP TABLE IF EXISTS `abplanguagetexts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abplanguagetexts` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `TenantId` int DEFAULT NULL,
  `LanguageName` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Source` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Key` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Value` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AbpLanguageTexts_TenantId_Source_LanguageName_Key` (`TenantId`,`Source`,`LanguageName`,`Key`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abpnotifications`
--

DROP TABLE IF EXISTS `abpnotifications`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abpnotifications` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `NotificationName` varchar(96) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Data` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `DataTypeName` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `EntityTypeName` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `EntityTypeAssemblyQualifiedName` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `EntityId` varchar(96) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Severity` tinyint unsigned NOT NULL,
  `UserIds` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ExcludedUserIds` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `TenantIds` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  `TargetNotifiers` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abpnotificationsubscriptions`
--

DROP TABLE IF EXISTS `abpnotificationsubscriptions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abpnotificationsubscriptions` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `UserId` bigint NOT NULL,
  `NotificationName` varchar(96) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `EntityTypeName` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `EntityTypeAssemblyQualifiedName` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `EntityId` varchar(96) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AbpNotificationSubscriptions_NotificationName_EntityTypeName~` (`NotificationName`,`EntityTypeName`,`EntityId`,`UserId`),
  KEY `IX_AbpNotificationSubscriptions_TenantId_NotificationName_Entit~` (`TenantId`,`NotificationName`,`EntityTypeName`,`EntityId`,`UserId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abporganizationunitroles`
--

DROP TABLE IF EXISTS `abporganizationunitroles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abporganizationunitroles` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `TenantId` int DEFAULT NULL,
  `RoleId` int NOT NULL,
  `OrganizationUnitId` bigint NOT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AbpOrganizationUnitRoles_TenantId_OrganizationUnitId` (`TenantId`,`OrganizationUnitId`),
  KEY `IX_AbpOrganizationUnitRoles_TenantId_RoleId` (`TenantId`,`RoleId`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abporganizationunits`
--

DROP TABLE IF EXISTS `abporganizationunits`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abporganizationunits` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `TenantId` int DEFAULT NULL,
  `ParentId` bigint DEFAULT NULL,
  `Code` varchar(95) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `DisplayName` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AbpOrganizationUnits_ParentId` (`ParentId`),
  KEY `IX_AbpOrganizationUnits_TenantId_Code` (`TenantId`,`Code`),
  CONSTRAINT `FK_AbpOrganizationUnits_AbpOrganizationUnits_ParentId` FOREIGN KEY (`ParentId`) REFERENCES `abporganizationunits` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=131 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abppermissions`
--

DROP TABLE IF EXISTS `abppermissions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abppermissions` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `TenantId` int DEFAULT NULL,
  `Name` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `IsGranted` tinyint(1) NOT NULL,
  `Discriminator` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `RoleId` int DEFAULT NULL,
  `UserId` bigint DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AbpPermissions_RoleId` (`RoleId`),
  KEY `IX_AbpPermissions_TenantId_Name` (`TenantId`,`Name`),
  KEY `IX_AbpPermissions_UserId` (`UserId`),
  CONSTRAINT `FK_AbpPermissions_AbpRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `abproles` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_AbpPermissions_AbpUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `abpusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=10313 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abppersistedgrants`
--

DROP TABLE IF EXISTS `abppersistedgrants`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abppersistedgrants` (
  `Id` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Type` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `SubjectId` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `SessionId` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ClientId` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Description` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `Expiration` datetime(6) DEFAULT NULL,
  `ConsumedTime` datetime(6) DEFAULT NULL,
  `Data` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AbpPersistedGrants_Expiration` (`Expiration`),
  KEY `IX_AbpPersistedGrants_SubjectId_ClientId_Type` (`SubjectId`,`ClientId`,`Type`),
  KEY `IX_AbpPersistedGrants_SubjectId_SessionId_Type` (`SubjectId`,`SessionId`,`Type`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abproleclaims`
--

DROP TABLE IF EXISTS `abproleclaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abproleclaims` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `TenantId` int DEFAULT NULL,
  `RoleId` int NOT NULL,
  `ClaimType` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ClaimValue` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AbpRoleClaims_RoleId` (`RoleId`),
  KEY `IX_AbpRoleClaims_TenantId_ClaimType` (`TenantId`,`ClaimType`),
  CONSTRAINT `FK_AbpRoleClaims_AbpRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `abproles` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abproles`
--

DROP TABLE IF EXISTS `abproles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abproles` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `TenantId` int DEFAULT NULL,
  `Name` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `DisplayName` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `IsStatic` tinyint(1) NOT NULL,
  `IsDefault` tinyint(1) NOT NULL,
  `NormalizedName` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ConcurrencyStamp` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AbpRoles_CreatorUserId` (`CreatorUserId`),
  KEY `IX_AbpRoles_DeleterUserId` (`DeleterUserId`),
  KEY `IX_AbpRoles_LastModifierUserId` (`LastModifierUserId`),
  KEY `IX_AbpRoles_TenantId_NormalizedName` (`TenantId`,`NormalizedName`),
  CONSTRAINT `FK_AbpRoles_AbpUsers_CreatorUserId` FOREIGN KEY (`CreatorUserId`) REFERENCES `abpusers` (`Id`),
  CONSTRAINT `FK_AbpRoles_AbpUsers_DeleterUserId` FOREIGN KEY (`DeleterUserId`) REFERENCES `abpusers` (`Id`),
  CONSTRAINT `FK_AbpRoles_AbpUsers_LastModifierUserId` FOREIGN KEY (`LastModifierUserId`) REFERENCES `abpusers` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=134 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abpsettings`
--

DROP TABLE IF EXISTS `abpsettings`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abpsettings` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `TenantId` int DEFAULT NULL,
  `UserId` bigint DEFAULT NULL,
  `Name` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Value` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_AbpSettings_TenantId_Name_UserId` (`TenantId`,`Name`,`UserId`),
  KEY `IX_AbpSettings_UserId` (`UserId`),
  CONSTRAINT `FK_AbpSettings_AbpUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `abpusers` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5267 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abptenantnotifications`
--

DROP TABLE IF EXISTS `abptenantnotifications`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abptenantnotifications` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `NotificationName` varchar(96) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Data` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `DataTypeName` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `EntityTypeName` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `EntityTypeAssemblyQualifiedName` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `EntityId` varchar(96) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Severity` tinyint unsigned NOT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AbpTenantNotifications_TenantId` (`TenantId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abptenants`
--

DROP TABLE IF EXISTS `abptenants`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abptenants` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `SubscriptionEndDateUtc` datetime(6) DEFAULT NULL,
  `IsInTrialPeriod` tinyint(1) NOT NULL,
  `CustomCssId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `LogoId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `LogoFileType` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `SubscriptionPaymentType` int NOT NULL,
  `ClientCode` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `TenancyName` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Name` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ConnectionString` varchar(1024) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `EditionId` int DEFAULT NULL,
  `IsDonorPay` tinyint(1) NOT NULL DEFAULT '0',
  `ClientPaymentType` int NOT NULL DEFAULT '0',
  `IsDeferDonorPerpetualPay` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  KEY `IX_AbpTenants_CreationTime` (`CreationTime`),
  KEY `IX_AbpTenants_CreatorUserId` (`CreatorUserId`),
  KEY `IX_AbpTenants_DeleterUserId` (`DeleterUserId`),
  KEY `IX_AbpTenants_EditionId` (`EditionId`),
  KEY `IX_AbpTenants_LastModifierUserId` (`LastModifierUserId`),
  KEY `IX_AbpTenants_SubscriptionEndDateUtc` (`SubscriptionEndDateUtc`),
  KEY `IX_AbpTenants_TenancyName` (`TenancyName`),
  CONSTRAINT `FK_AbpTenants_AbpEditions_EditionId` FOREIGN KEY (`EditionId`) REFERENCES `abpeditions` (`Id`),
  CONSTRAINT `FK_AbpTenants_AbpUsers_CreatorUserId` FOREIGN KEY (`CreatorUserId`) REFERENCES `abpusers` (`Id`),
  CONSTRAINT `FK_AbpTenants_AbpUsers_DeleterUserId` FOREIGN KEY (`DeleterUserId`) REFERENCES `abpusers` (`Id`),
  CONSTRAINT `FK_AbpTenants_AbpUsers_LastModifierUserId` FOREIGN KEY (`LastModifierUserId`) REFERENCES `abpusers` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=49 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abpuseraccounts`
--

DROP TABLE IF EXISTS `abpuseraccounts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abpuseraccounts` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `TenantId` int DEFAULT NULL,
  `UserId` bigint NOT NULL,
  `UserLinkId` bigint DEFAULT NULL,
  `UserName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `EmailAddress` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AbpUserAccounts_EmailAddress` (`EmailAddress`),
  KEY `IX_AbpUserAccounts_TenantId_EmailAddress` (`TenantId`,`EmailAddress`),
  KEY `IX_AbpUserAccounts_TenantId_UserId` (`TenantId`,`UserId`),
  KEY `IX_AbpUserAccounts_TenantId_UserName` (`TenantId`,`UserName`),
  KEY `IX_AbpUserAccounts_UserName` (`UserName`)
) ENGINE=InnoDB AUTO_INCREMENT=16678 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abpuserclaims`
--

DROP TABLE IF EXISTS `abpuserclaims`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abpuserclaims` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `TenantId` int DEFAULT NULL,
  `UserId` bigint NOT NULL,
  `ClaimType` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ClaimValue` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AbpUserClaims_TenantId_ClaimType` (`TenantId`,`ClaimType`),
  KEY `IX_AbpUserClaims_UserId` (`UserId`),
  CONSTRAINT `FK_AbpUserClaims_AbpUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `abpusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abpuserloginattempts`
--

DROP TABLE IF EXISTS `abpuserloginattempts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abpuserloginattempts` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `TenantId` int DEFAULT NULL,
  `TenancyName` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `UserId` bigint DEFAULT NULL,
  `UserNameOrEmailAddress` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ClientIpAddress` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ClientName` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `BrowserInfo` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Result` tinyint unsigned NOT NULL,
  `CreationTime` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AbpUserLoginAttempts_TenancyName_UserNameOrEmailAddress_Resu~` (`TenancyName`,`UserNameOrEmailAddress`,`Result`),
  KEY `IX_AbpUserLoginAttempts_UserId_TenantId` (`UserId`,`TenantId`)
) ENGINE=InnoDB AUTO_INCREMENT=349888 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abpuserlogins`
--

DROP TABLE IF EXISTS `abpuserlogins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abpuserlogins` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `TenantId` int DEFAULT NULL,
  `UserId` bigint NOT NULL,
  `LoginProvider` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProviderKey` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_AbpUserLogins_ProviderKey_TenantId` (`ProviderKey`,`TenantId`),
  KEY `IX_AbpUserLogins_TenantId_LoginProvider_ProviderKey` (`TenantId`,`LoginProvider`,`ProviderKey`),
  KEY `IX_AbpUserLogins_TenantId_UserId` (`TenantId`,`UserId`),
  KEY `IX_AbpUserLogins_UserId` (`UserId`),
  CONSTRAINT `FK_AbpUserLogins_AbpUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `abpusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abpusernotifications`
--

DROP TABLE IF EXISTS `abpusernotifications`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abpusernotifications` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `UserId` bigint NOT NULL,
  `TenantNotificationId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `State` int NOT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `TargetNotifiers` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  KEY `IX_AbpUserNotifications_UserId_State_CreationTime` (`UserId`,`State`,`CreationTime`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abpuserorganizationunits`
--

DROP TABLE IF EXISTS `abpuserorganizationunits`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abpuserorganizationunits` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `TenantId` int DEFAULT NULL,
  `UserId` bigint NOT NULL,
  `OrganizationUnitId` bigint NOT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AbpUserOrganizationUnits_TenantId_OrganizationUnitId` (`TenantId`,`OrganizationUnitId`),
  KEY `IX_AbpUserOrganizationUnits_TenantId_UserId` (`TenantId`,`UserId`),
  KEY `IX_AbpUserOrganizationUnits_UserId` (`UserId`),
  CONSTRAINT `FK_AbpUserOrganizationUnits_AbpUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `abpusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=156 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abpuserroles`
--

DROP TABLE IF EXISTS `abpuserroles`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abpuserroles` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `TenantId` int DEFAULT NULL,
  `UserId` bigint NOT NULL,
  `RoleId` int NOT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AbpUserRoles_TenantId_RoleId` (`TenantId`,`RoleId`),
  KEY `IX_AbpUserRoles_TenantId_UserId` (`TenantId`,`UserId`),
  KEY `IX_AbpUserRoles_UserId` (`UserId`),
  CONSTRAINT `FK_AbpUserRoles_AbpUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `abpusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=501506 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abpusers`
--

DROP TABLE IF EXISTS `abpusers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abpusers` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `ProfilePictureId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `ShouldChangePasswordOnNextLogin` tinyint(1) NOT NULL,
  `SignInTokenExpireTimeUtc` datetime(6) DEFAULT NULL,
  `SignInToken` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `GoogleAuthenticatorKey` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `AuthenticationSource` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `UserName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `EmailAddress` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Name` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Surname` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Password` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `EmailConfirmationCode` varchar(328) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `PasswordResetCode` varchar(328) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `LockoutEndDateUtc` datetime(6) DEFAULT NULL,
  `AccessFailedCount` int NOT NULL,
  `IsLockoutEnabled` tinyint(1) NOT NULL,
  `PhoneNumber` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `IsPhoneNumberConfirmed` tinyint(1) NOT NULL,
  `SecurityStamp` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `IsTwoFactorEnabled` tinyint(1) NOT NULL,
  `IsEmailConfirmed` tinyint(1) NOT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `NormalizedUserName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `NormalizedEmailAddress` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ConcurrencyStamp` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `MiddleName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Address` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `City` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `State` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `SuiteApt` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Zip` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `DateOfBirth` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `IsPaid` tinyint(1) NOT NULL DEFAULT '0',
  `IsAlwaysDonor` tinyint(1) NOT NULL DEFAULT '0',
  `DonorPayPromptDelayUntil` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  PRIMARY KEY (`Id`),
  KEY `IX_AbpUsers_CreatorUserId` (`CreatorUserId`),
  KEY `IX_AbpUsers_DeleterUserId` (`DeleterUserId`),
  KEY `IX_AbpUsers_LastModifierUserId` (`LastModifierUserId`),
  KEY `IX_AbpUsers_TenantId_NormalizedEmailAddress` (`TenantId`,`NormalizedEmailAddress`),
  KEY `IX_AbpUsers_TenantId_NormalizedUserName` (`TenantId`,`NormalizedUserName`),
  CONSTRAINT `FK_AbpUsers_AbpUsers_CreatorUserId` FOREIGN KEY (`CreatorUserId`) REFERENCES `abpusers` (`Id`),
  CONSTRAINT `FK_AbpUsers_AbpUsers_DeleterUserId` FOREIGN KEY (`DeleterUserId`) REFERENCES `abpusers` (`Id`),
  CONSTRAINT `FK_AbpUsers_AbpUsers_LastModifierUserId` FOREIGN KEY (`LastModifierUserId`) REFERENCES `abpusers` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=17099 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abpusertokens`
--

DROP TABLE IF EXISTS `abpusertokens`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abpusertokens` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `TenantId` int DEFAULT NULL,
  `UserId` bigint NOT NULL,
  `LoginProvider` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Name` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Value` varchar(512) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ExpireDate` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AbpUserTokens_TenantId_UserId` (`TenantId`,`UserId`),
  KEY `IX_AbpUserTokens_UserId` (`UserId`),
  CONSTRAINT `FK_AbpUserTokens_AbpUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `abpusers` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abpwebhookevents`
--

DROP TABLE IF EXISTS `abpwebhookevents`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abpwebhookevents` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `WebhookName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Data` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `CreationTime` datetime(6) NOT NULL,
  `TenantId` int DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abpwebhooksendattempts`
--

DROP TABLE IF EXISTS `abpwebhooksendattempts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abpwebhooksendattempts` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `WebhookEventId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `WebhookSubscriptionId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Response` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ResponseStatusCode` int DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `TenantId` int DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AbpWebhookSendAttempts_WebhookEventId` (`WebhookEventId`),
  CONSTRAINT `FK_AbpWebhookSendAttempts_AbpWebhookEvents_WebhookEventId` FOREIGN KEY (`WebhookEventId`) REFERENCES `abpwebhookevents` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `abpwebhooksubscriptions`
--

DROP TABLE IF EXISTS `abpwebhooksubscriptions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `abpwebhooksubscriptions` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `WebhookUri` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Secret` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `Webhooks` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Headers` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `appbinaryobjects`
--

DROP TABLE IF EXISTS `appbinaryobjects`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `appbinaryobjects` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Bytes` longblob NOT NULL,
  `FileName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `IsFile` tinyint(1) NOT NULL DEFAULT '0',
  `Metadata` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `OriginalFileName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`),
  KEY `IX_AppBinaryObjects_TenantId` (`TenantId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `appchatmessages`
--

DROP TABLE IF EXISTS `appchatmessages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `appchatmessages` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `UserId` bigint NOT NULL,
  `TenantId` int DEFAULT NULL,
  `TargetUserId` bigint NOT NULL,
  `TargetTenantId` int DEFAULT NULL,
  `Message` varchar(4096) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `Side` int NOT NULL,
  `ReadState` int NOT NULL,
  `ReceiverReadState` int NOT NULL,
  `SharedMessageId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AppChatMessages_TargetTenantId_TargetUserId_ReadState` (`TargetTenantId`,`TargetUserId`,`ReadState`),
  KEY `IX_AppChatMessages_TargetTenantId_UserId_ReadState` (`TargetTenantId`,`UserId`,`ReadState`),
  KEY `IX_AppChatMessages_TenantId_TargetUserId_ReadState` (`TenantId`,`TargetUserId`,`ReadState`),
  KEY `IX_AppChatMessages_TenantId_UserId_ReadState` (`TenantId`,`UserId`,`ReadState`)
) ENGINE=InnoDB AUTO_INCREMENT=163 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `appfriendships`
--

DROP TABLE IF EXISTS `appfriendships`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `appfriendships` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `UserId` bigint NOT NULL,
  `TenantId` int DEFAULT NULL,
  `FriendUserId` bigint NOT NULL,
  `FriendTenantId` int DEFAULT NULL,
  `FriendUserName` varchar(256) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `FriendTenancyName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `FriendProfilePictureId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `State` int NOT NULL,
  `CreationTime` datetime(6) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AppFriendships_FriendTenantId_FriendUserId` (`FriendTenantId`,`FriendUserId`),
  KEY `IX_AppFriendships_FriendTenantId_UserId` (`FriendTenantId`,`UserId`),
  KEY `IX_AppFriendships_TenantId_FriendUserId` (`TenantId`,`FriendUserId`),
  KEY `IX_AppFriendships_TenantId_UserId` (`TenantId`,`UserId`)
) ENGINE=InnoDB AUTO_INCREMENT=247 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `appinvoices`
--

DROP TABLE IF EXISTS `appinvoices`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `appinvoices` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `InvoiceNo` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `InvoiceDate` datetime(6) NOT NULL,
  `TenantLegalName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `TenantAddress` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `TenantTaxNo` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `apprecentpasswords`
--

DROP TABLE IF EXISTS `apprecentpasswords`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `apprecentpasswords` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `UserId` bigint NOT NULL,
  `Password` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `appsubscriptionpayments`
--

DROP TABLE IF EXISTS `appsubscriptionpayments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `appsubscriptionpayments` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Gateway` int NOT NULL,
  `Amount` decimal(65,30) NOT NULL,
  `Status` int NOT NULL,
  `EditionId` int NOT NULL,
  `TenantId` int NOT NULL,
  `DayCount` int NOT NULL,
  `PaymentPeriodType` int DEFAULT NULL,
  `ExternalPaymentId` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `InvoiceNo` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `IsRecurring` tinyint(1) NOT NULL,
  `SuccessUrl` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ErrorUrl` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `EditionPaymentType` int NOT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AppSubscriptionPayments_EditionId` (`EditionId`),
  KEY `IX_AppSubscriptionPayments_ExternalPaymentId_Gateway` (`ExternalPaymentId`,`Gateway`),
  KEY `IX_AppSubscriptionPayments_Status_CreationTime` (`Status`,`CreationTime`),
  CONSTRAINT `FK_AppSubscriptionPayments_AbpEditions_EditionId` FOREIGN KEY (`EditionId`) REFERENCES `abpeditions` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `appsubscriptionpaymentsextensiondata`
--

DROP TABLE IF EXISTS `appsubscriptionpaymentsextensiondata`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `appsubscriptionpaymentsextensiondata` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `SubscriptionPaymentId` bigint NOT NULL,
  `Key` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `Value` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `IsDeleted` tinyint(1) NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `IX_AppSubscriptionPaymentsExtensionData_SubscriptionPaymentId_K~` (`SubscriptionPaymentId`,`Key`,`IsDeleted`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `appuserdelegations`
--

DROP TABLE IF EXISTS `appuserdelegations`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `appuserdelegations` (
  `Id` bigint NOT NULL AUTO_INCREMENT,
  `SourceUserId` bigint NOT NULL,
  `TargetUserId` bigint NOT NULL,
  `TenantId` int DEFAULT NULL,
  `StartTime` datetime(6) NOT NULL,
  `EndTime` datetime(6) NOT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_AppUserDelegations_TenantId_SourceUserId` (`TenantId`,`SourceUserId`),
  KEY `IX_AppUserDelegations_TenantId_TargetUserId` (`TenantId`,`TargetUserId`)
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `codetypes`
--

DROP TABLE IF EXISTS `codetypes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `codetypes` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `Name` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreationTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `CreatorUserId` bigint DEFAULT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_CodeTypes_TenantId` (`TenantId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `cohorts`
--

DROP TABLE IF EXISTS `cohorts`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cohorts` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `DefaultCohort` tinyint(1) NOT NULL,
  `TenantDepartmentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `CreatorUserId` bigint DEFAULT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Cohorts_TenantDepartmentId` (`TenantDepartmentId`),
  KEY `IX_Cohorts_TenantId` (`TenantId`),
  CONSTRAINT `FK_Cohorts_TenantDepartments_TenantDepartmentId` FOREIGN KEY (`TenantDepartmentId`) REFERENCES `tenantdepartments` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `cohortusers`
--

DROP TABLE IF EXISTS `cohortusers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cohortusers` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `CohortId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `UserId` bigint NOT NULL,
  `CreationTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `CreatorUserId` bigint DEFAULT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_CohortUsers_CohortId` (`CohortId`),
  KEY `IX_CohortUsers_TenantId` (`TenantId`),
  KEY `IX_CohortUsers_UserId` (`UserId`),
  CONSTRAINT `FK_CohortUsers_AbpUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `abpusers` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_CohortUsers_Cohorts_CohortId` FOREIGN KEY (`CohortId`) REFERENCES `cohorts` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `confirmationvalues`
--

DROP TABLE IF EXISTS `confirmationvalues`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `confirmationvalues` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `ScreenValue` double NOT NULL,
  `ConfirmValue` double NOT NULL,
  `UnitOfMeasurement` int NOT NULL,
  `DrugId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TestCategoryId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `CreationTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `CreatorUserId` bigint DEFAULT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_ConfirmationValues_DrugId` (`DrugId`),
  KEY `IX_ConfirmationValues_TestCategoryId` (`TestCategoryId`),
  CONSTRAINT `FK_ConfirmationValues_Drugs_DrugId` FOREIGN KEY (`DrugId`) REFERENCES `drugs` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_ConfirmationValues_TestCategories_TestCategoryId` FOREIGN KEY (`TestCategoryId`) REFERENCES `testcategories` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `departmentusers`
--

DROP TABLE IF EXISTS `departmentusers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `departmentusers` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `UserId` bigint DEFAULT NULL,
  `TenantDepartmentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `CreatorUserId` bigint DEFAULT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_DepartmentUsers_TenantDepartmentId` (`TenantDepartmentId`),
  KEY `IX_DepartmentUsers_TenantId` (`TenantId`),
  KEY `IX_DepartmentUsers_UserId` (`UserId`),
  CONSTRAINT `FK_DepartmentUsers_AbpUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `abpusers` (`Id`),
  CONSTRAINT `FK_DepartmentUsers_TenantDepartments_TenantDepartmentId` FOREIGN KEY (`TenantDepartmentId`) REFERENCES `tenantdepartments` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `deptcodes`
--

DROP TABLE IF EXISTS `deptcodes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `deptcodes` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `Code` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CodeTypeId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantDepartmentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `CreationTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `CreatorUserId` bigint DEFAULT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_DeptCodes_CodeTypeId` (`CodeTypeId`),
  KEY `IX_DeptCodes_TenantDepartmentId` (`TenantDepartmentId`),
  KEY `IX_DeptCodes_TenantId` (`TenantId`),
  CONSTRAINT `FK_DeptCodes_CodeTypes_CodeTypeId` FOREIGN KEY (`CodeTypeId`) REFERENCES `codetypes` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_DeptCodes_TenantDepartments_TenantDepartmentId` FOREIGN KEY (`TenantDepartmentId`) REFERENCES `tenantdepartments` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `drugpanels`
--

DROP TABLE IF EXISTS `drugpanels`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `drugpanels` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DrugId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `PanelId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `CreationTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `CreatorUserId` bigint DEFAULT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_DrugPanels_DrugId` (`DrugId`),
  KEY `IX_DrugPanels_PanelId` (`PanelId`),
  CONSTRAINT `FK_DrugPanels_Drugs_DrugId` FOREIGN KEY (`DrugId`) REFERENCES `drugs` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_DrugPanels_Panels_PanelId` FOREIGN KEY (`PanelId`) REFERENCES `panels` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `drugs`
--

DROP TABLE IF EXISTS `drugs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `drugs` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Name` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Code` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreationTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `CreatorUserId` bigint DEFAULT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `drugtestcategories`
--

DROP TABLE IF EXISTS `drugtestcategories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `drugtestcategories` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `DrugId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `PanelId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TestCategoryId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `CreationTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `CreatorUserId` bigint DEFAULT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_DrugTestCategories_DrugId` (`DrugId`),
  KEY `IX_DrugTestCategories_PanelId` (`PanelId`),
  KEY `IX_DrugTestCategories_TestCategoryId` (`TestCategoryId`),
  CONSTRAINT `FK_DrugTestCategories_Drugs_DrugId` FOREIGN KEY (`DrugId`) REFERENCES `drugs` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_DrugTestCategories_Panels_PanelId` FOREIGN KEY (`PanelId`) REFERENCES `panels` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_DrugTestCategories_TestCategories_TestCategoryId` FOREIGN KEY (`TestCategoryId`) REFERENCES `testcategories` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `hospitals`
--

DROP TABLE IF EXISTS `hospitals`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hospitals` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `TenantId` int DEFAULT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PrimaryContact` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PrimaryContactPhone` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PrimaryContactEmail` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  `Address1` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Address2` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `City` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `State` int NOT NULL DEFAULT '0',
  `ZipCode` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Hospitals_TenantId` (`TenantId`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `ledgerentries`
--

DROP TABLE IF EXISTS `ledgerentries`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ledgerentries` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ServiceType` int NOT NULL,
  `Amount` double NOT NULL,
  `DiscountAmount` double NOT NULL,
  `TotalPrice` double NOT NULL,
  `PaymentPeriodType` int NOT NULL,
  `ExpirationDate` datetime(6) DEFAULT NULL,
  `TransactionName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `TransactionKey` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `TransactionId` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Settled` tinyint(1) NOT NULL,
  `AmountDue` double NOT NULL,
  `PaymentToken` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `AuthNetCustomerProfileId` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `AuthNetCustomerPaymentProfileId` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `AuthNetCustomerAddressId` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `AccountNumber` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Note` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `MetaData` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `AuthCode` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ReferenceTransactionId` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `TransactionHash` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `AccountType` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `TransactionCode` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `TransactionMessage` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `AuthNetTransHashSha2` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `AuthNetNetworkTransId` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `UserId` bigint DEFAULT NULL,
  `TenantDocumentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `CohortId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `AvailableUserBalance` bigint NOT NULL DEFAULT '0',
  `PaidAmount` bigint NOT NULL DEFAULT '0',
  `PaidInCash` bigint NOT NULL DEFAULT '0',
  `CardNameOnCard` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CardZipCode` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `BalanceForward` double NOT NULL DEFAULT '0',
  `CardLastFour` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `IsRefund` tinyint(1) NOT NULL DEFAULT '0',
  `PaymentDate` datetime(6) DEFAULT NULL,
  `PaymentMethod` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `UserPurchaseId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_LedgerEntries_CohortId` (`CohortId`),
  KEY `IX_LedgerEntries_TenantDocumentId` (`TenantDocumentId`),
  KEY `IX_LedgerEntries_TenantId` (`TenantId`),
  KEY `IX_LedgerEntries_UserId` (`UserId`),
  KEY `IX_LedgerEntries_UserPurchaseId` (`UserPurchaseId`),
  CONSTRAINT `FK_LedgerEntries_AbpUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `abpusers` (`Id`),
  CONSTRAINT `FK_LedgerEntries_Cohorts_CohortId` FOREIGN KEY (`CohortId`) REFERENCES `cohorts` (`Id`),
  CONSTRAINT `FK_LedgerEntries_TenantDocuments_TenantDocumentId` FOREIGN KEY (`TenantDocumentId`) REFERENCES `tenantdocuments` (`Id`),
  CONSTRAINT `FK_LedgerEntries_UserPurchases_UserPurchaseId` FOREIGN KEY (`UserPurchaseId`) REFERENCES `userpurchases` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `ledgerentrydetails`
--

DROP TABLE IF EXISTS `ledgerentrydetails`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ledgerentrydetails` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `Note` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Amount` double NOT NULL,
  `Discount` decimal(65,30) NOT NULL,
  `DiscountAmount` double NOT NULL,
  `MetaData` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `LedgerEntryId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `SurpathServiceId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `CreationTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `CreatorUserId` bigint DEFAULT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  `TenantSurpathServiceId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `AmountPaid` double NOT NULL DEFAULT '0',
  `DatePaidOn` datetime(6) DEFAULT NULL,
  `UserPurchaseId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_LedgerEntryDetails_LedgerEntryId` (`LedgerEntryId`),
  KEY `IX_LedgerEntryDetails_SurpathServiceId` (`SurpathServiceId`),
  KEY `IX_LedgerEntryDetails_TenantId` (`TenantId`),
  KEY `IX_LedgerEntryDetails_TenantSurpathServiceId` (`TenantSurpathServiceId`),
  KEY `IX_LedgerEntryDetails_UserPurchaseId` (`UserPurchaseId`),
  CONSTRAINT `FK_LedgerEntryDetails_LedgerEntries_LedgerEntryId` FOREIGN KEY (`LedgerEntryId`) REFERENCES `ledgerentries` (`Id`),
  CONSTRAINT `FK_LedgerEntryDetails_SurpathServices_SurpathServiceId` FOREIGN KEY (`SurpathServiceId`) REFERENCES `surpathservices` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_LedgerEntryDetails_TenantSurpathServices_TenantSurpathServic~` FOREIGN KEY (`TenantSurpathServiceId`) REFERENCES `tenantsurpathservices` (`Id`),
  CONSTRAINT `FK_LedgerEntryDetails_UserPurchases_UserPurchaseId` FOREIGN KEY (`UserPurchaseId`) REFERENCES `userpurchases` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `legaldocuments`
--

DROP TABLE IF EXISTS `legaldocuments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `legaldocuments` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `FileId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `Type` int NOT NULL,
  `ExternalUrl` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `FileName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `ViewUrl` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_LegalDocuments_TenantId` (`TenantId`),
  KEY `IX_LegalDocuments_Type` (`Type`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `medicalunits`
--

DROP TABLE IF EXISTS `medicalunits`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `medicalunits` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `TenantId` int DEFAULT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PrimaryContact` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PrimaryContactPhone` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `PrimaryContactEmail` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `HospitalId` int NOT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  `Address1` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Address2` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `City` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `State` int NOT NULL DEFAULT '0',
  `ZipCode` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_MedicalUnits_HospitalId` (`HospitalId`),
  KEY `IX_MedicalUnits_TenantId` (`TenantId`),
  CONSTRAINT `FK_MedicalUnits_Hospitals_HospitalId` FOREIGN KEY (`HospitalId`) REFERENCES `hospitals` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=37 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `migrationauditlogs`
--

DROP TABLE IF EXISTS `migrationauditlogs`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `migrationauditlogs` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `MigrationId` varchar(100) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `MigrationType` varchar(50) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Status` varchar(20) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CohortId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `CohortName` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `SourceDepartmentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `SourceDepartmentName` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `TargetDepartmentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `TargetDepartmentName` varchar(200) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `IsNewDepartment` tinyint(1) NOT NULL,
  `AffectedUsersCount` int NOT NULL,
  `AffectedRecordsCount` int NOT NULL,
  `RequirementCategoriesCount` int NOT NULL,
  `MappingDecisionsJson` longtext,
  `BeforeStateJson` longtext,
  `AfterStateJson` longtext,
  `StartedAt` datetime(6) DEFAULT NULL,
  `CompletedAt` datetime(6) DEFAULT NULL,
  `DurationMs` bigint DEFAULT NULL,
  `ErrorMessage` varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `ErrorDetails` longtext,
  `MetadataJson` longtext,
  `CanRollback` tinyint(1) NOT NULL,
  `IsRolledBack` tinyint(1) NOT NULL,
  `RolledBackAt` datetime(6) DEFAULT NULL,
  `RolledBackByUserId` bigint DEFAULT NULL,
  `RollbackReason` varchar(1000) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_MigrationAuditLogs_CohortId` (`CohortId`),
  KEY `IX_MigrationAuditLogs_CompletedAt` (`CompletedAt`),
  KEY `IX_MigrationAuditLogs_MigrationId` (`MigrationId`),
  KEY `IX_MigrationAuditLogs_SourceDepartmentId` (`SourceDepartmentId`),
  KEY `IX_MigrationAuditLogs_StartedAt` (`StartedAt`),
  KEY `IX_MigrationAuditLogs_Status` (`Status`),
  KEY `IX_MigrationAuditLogs_TargetDepartmentId` (`TargetDepartmentId`),
  KEY `IX_MigrationAuditLogs_TenantId` (`TenantId`),
  CONSTRAINT `FK_MigrationAuditLogs_Cohorts_CohortId` FOREIGN KEY (`CohortId`) REFERENCES `cohorts` (`Id`),
  CONSTRAINT `FK_MigrationAuditLogs_TenantDepartments_SourceDepartmentId` FOREIGN KEY (`SourceDepartmentId`) REFERENCES `tenantdepartments` (`Id`),
  CONSTRAINT `FK_MigrationAuditLogs_TenantDepartments_TargetDepartmentId` FOREIGN KEY (`TargetDepartmentId`) REFERENCES `tenantdepartments` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `panels`
--

DROP TABLE IF EXISTS `panels`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `panels` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Name` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Cost` double NOT NULL,
  `Description` varchar(250) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci DEFAULT NULL,
  `TestCategoryId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `CreatorUserId` bigint DEFAULT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Panels_TestCategoryId` (`TestCategoryId`),
  CONSTRAINT `FK_Panels_TestCategories_TestCategoryId` FOREIGN KEY (`TestCategoryId`) REFERENCES `testcategories` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pidtypes`
--

DROP TABLE IF EXISTS `pidtypes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pidtypes` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `MaskPid` tinyint(1) NOT NULL,
  `PidRegex` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `CreatedOn` datetime(6) NOT NULL,
  `ModifiedOn` datetime(6) NOT NULL,
  `CreatedBy` bigint NOT NULL,
  `LastModifiedBy` bigint NOT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `PidInputMask` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Required` tinyint(1) NOT NULL DEFAULT '0',
  `CreationTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `CreatorUserId` bigint DEFAULT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_PidTypes_TenantId` (`TenantId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `processedtransactions`
--

DROP TABLE IF EXISTS `processedtransactions`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `processedtransactions` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `TenantId` int DEFAULT NULL,
  `TransactionId` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=4999 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `recordcategories`
--

DROP TABLE IF EXISTS `recordcategories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `recordcategories` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Instructions` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `RecordRequirementId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `RecordCategoryRuleId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `CreatorUserId` bigint DEFAULT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_RecordCategories_RecordCategoryRuleId` (`RecordCategoryRuleId`),
  KEY `IX_RecordCategories_RecordRequirementId` (`RecordRequirementId`),
  KEY `IX_RecordCategories_TenantId` (`TenantId`),
  CONSTRAINT `FK_RecordCategories_RecordCategoryRules_RecordCategoryRuleId` FOREIGN KEY (`RecordCategoryRuleId`) REFERENCES `recordcategoryrules` (`Id`),
  CONSTRAINT `FK_RecordCategories_RecordRequirements_RecordRequirementId` FOREIGN KEY (`RecordRequirementId`) REFERENCES `recordrequirements` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `recordcategoryrules`
--

DROP TABLE IF EXISTS `recordcategoryrules`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `recordcategoryrules` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Notify` tinyint(1) NOT NULL,
  `ExpireInDays` int NOT NULL,
  `WarnDaysBeforeFirst` int NOT NULL,
  `Expires` tinyint(1) NOT NULL,
  `Required` tinyint(1) NOT NULL,
  `WarnDaysBeforeSecond` int NOT NULL,
  `WarnDaysBeforeFinal` int NOT NULL,
  `CreationTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `CreatorUserId` bigint DEFAULT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  `IsSurpathOnly` tinyint(1) NOT NULL DEFAULT '0',
  `MetaData` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `TemplateRuleId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
  `FinalWarnStatusId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `FirstWarnStatusId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `SecondWarnStatusId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `ExpiredStatusId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_RecordCategoryRules_TenantId` (`TenantId`),
  KEY `IX_RecordCategoryRules_FinalWarnStatusId` (`FinalWarnStatusId`),
  KEY `IX_RecordCategoryRules_FirstWarnStatusId` (`FirstWarnStatusId`),
  KEY `IX_RecordCategoryRules_SecondWarnStatusId` (`SecondWarnStatusId`),
  KEY `IX_RecordCategoryRules_ExpiredStatusId` (`ExpiredStatusId`),
  CONSTRAINT `FK_RecordCategoryRules_RecordStatuses_ExpiredStatusId` FOREIGN KEY (`ExpiredStatusId`) REFERENCES `recordstatuses` (`Id`),
  CONSTRAINT `FK_RecordCategoryRules_RecordStatuses_FinalWarnStatusId` FOREIGN KEY (`FinalWarnStatusId`) REFERENCES `recordstatuses` (`Id`),
  CONSTRAINT `FK_RecordCategoryRules_RecordStatuses_FirstWarnStatusId` FOREIGN KEY (`FirstWarnStatusId`) REFERENCES `recordstatuses` (`Id`),
  CONSTRAINT `FK_RecordCategoryRules_RecordStatuses_SecondWarnStatusId` FOREIGN KEY (`SecondWarnStatusId`) REFERENCES `recordstatuses` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `recordnotes`
--

DROP TABLE IF EXISTS `recordnotes`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `recordnotes` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `Note` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Created` datetime(6) NOT NULL,
  `AuthorizedOnly` tinyint(1) NOT NULL,
  `HostOnly` tinyint(1) NOT NULL,
  `SendNotification` tinyint(1) NOT NULL,
  `RecordStateId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `UserId` bigint DEFAULT NULL,
  `NotifyUserId` bigint DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `CreatorUserId` bigint DEFAULT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_RecordNotes_NotifyUserId` (`NotifyUserId`),
  KEY `IX_RecordNotes_RecordStateId` (`RecordStateId`),
  KEY `IX_RecordNotes_TenantId` (`TenantId`),
  KEY `IX_RecordNotes_UserId` (`UserId`),
  CONSTRAINT `FK_RecordNotes_AbpUsers_NotifyUserId` FOREIGN KEY (`NotifyUserId`) REFERENCES `abpusers` (`Id`),
  CONSTRAINT `FK_RecordNotes_AbpUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `abpusers` (`Id`),
  CONSTRAINT `FK_RecordNotes_RecordStates_RecordStateId` FOREIGN KEY (`RecordStateId`) REFERENCES `recordstates` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `recordrequirements`
--

DROP TABLE IF EXISTS `recordrequirements`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `recordrequirements` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Metadata` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `CohortId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `TenantDepartmentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `CreatorUserId` bigint DEFAULT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  `IsSurpathOnly` tinyint(1) NOT NULL DEFAULT '0',
  `SurpathServiceId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `TenantSurpathServiceId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_RecordRequirements_TenantId` (`TenantId`),
  KEY `IX_RecordRequirements_CohortId` (`CohortId`),
  KEY `IX_RecordRequirements_TenantDepartmentId` (`TenantDepartmentId`),
  KEY `IX_RecordRequirements_SurpathServiceId` (`SurpathServiceId`),
  KEY `IX_RecordRequirements_TenantSurpathServiceId` (`TenantSurpathServiceId`),
  CONSTRAINT `FK_RecordRequirements_Cohorts_CohortId` FOREIGN KEY (`CohortId`) REFERENCES `cohorts` (`Id`),
  CONSTRAINT `FK_RecordRequirements_SurpathServices_SurpathServiceId` FOREIGN KEY (`SurpathServiceId`) REFERENCES `surpathservices` (`Id`),
  CONSTRAINT `FK_RecordRequirements_TenantDepartments_TenantDepartmentId` FOREIGN KEY (`TenantDepartmentId`) REFERENCES `tenantdepartments` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `records`
--

DROP TABLE IF EXISTS `records`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `records` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `filedata` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `filename` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `physicalfilepath` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `metadata` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `BinaryObjId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantDocumentCategoryId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `DateLastUpdated` datetime(6) DEFAULT NULL,
  `DateUploaded` datetime(6) DEFAULT NULL,
  `InstructionsConfirmed` tinyint(1) NOT NULL DEFAULT '0',
  `CreationTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `CreatorUserId` bigint DEFAULT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  `EffectiveDate` datetime(6) DEFAULT NULL,
  `ExpirationDate` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Records_TenantDocumentCategoryId` (`TenantDocumentCategoryId`),
  KEY `IX_Records_TenantId` (`TenantId`),
  CONSTRAINT `FK_Records_TenantDocumentCategories_TenantDocumentCategoryId` FOREIGN KEY (`TenantDocumentCategoryId`) REFERENCES `tenantdocumentcategories` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `recordstates`
--

DROP TABLE IF EXISTS `recordstates`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `recordstates` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `State` int NOT NULL,
  `Notes` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `RecordId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `RecordCategoryId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `UserId` bigint DEFAULT NULL,
  `RecordStatusId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `CreationTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `CreatorUserId` bigint DEFAULT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  `IsArchived` tinyint(1) NOT NULL DEFAULT '0',
  `ArchivedByUserId` bigint DEFAULT NULL,
  `ArchivedTime` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_RecordStates_RecordCategoryId` (`RecordCategoryId`),
  KEY `IX_RecordStates_RecordId` (`RecordId`),
  KEY `IX_RecordStates_RecordStatusId` (`RecordStatusId`),
  KEY `IX_RecordStates_TenantId` (`TenantId`),
  KEY `IX_RecordStates_UserId` (`UserId`),
  CONSTRAINT `FK_RecordStates_AbpUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `abpusers` (`Id`),
  CONSTRAINT `FK_RecordStates_RecordCategories_RecordCategoryId` FOREIGN KEY (`RecordCategoryId`) REFERENCES `recordcategories` (`Id`),
  CONSTRAINT `FK_RecordStates_Records_RecordId` FOREIGN KEY (`RecordId`) REFERENCES `records` (`Id`),
  CONSTRAINT `FK_RecordStates_RecordStatuses_RecordStatusId` FOREIGN KEY (`RecordStatusId`) REFERENCES `recordstatuses` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `recordstatuses`
--

DROP TABLE IF EXISTS `recordstatuses`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `recordstatuses` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `StatusName` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `HtmlColor` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `TenantDepartmentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `CSSCLass` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `IsDefault` tinyint(1) NOT NULL DEFAULT '0',
  `RequireNoteOnSet` tinyint(1) NOT NULL DEFAULT '0',
  `CreationTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `CreatorUserId` bigint DEFAULT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  `IsSurpathServiceStatus` tinyint(1) NOT NULL DEFAULT '0',
  `ComplianceImpact` int NOT NULL DEFAULT '0',
  `TemplateServiceId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL DEFAULT '00000000-0000-0000-0000-000000000000',
  PRIMARY KEY (`Id`),
  KEY `IX_RecordStatuses_TenantId` (`TenantId`),
  KEY `IX_RecordStatuses_TenantDepartmentId` (`TenantDepartmentId`),
  CONSTRAINT `FK_RecordStatuses_TenantDepartments_TenantDepartmentId` FOREIGN KEY (`TenantDepartmentId`) REFERENCES `tenantdepartments` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `rotationslots`
--

DROP TABLE IF EXISTS `rotationslots`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `rotationslots` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `TenantId` int DEFAULT NULL,
  `SlotId` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `AvailableSlots` int NOT NULL,
  `ShiftStartDate` datetime(6) NOT NULL,
  `ShiftEndDate` datetime(6) NOT NULL,
  `ShiftStartTime` datetime(6) NOT NULL,
  `ShiftEndTime` datetime(6) NOT NULL,
  `ShiftHours` decimal(65,30) NOT NULL,
  `NotifyHospital` tinyint(1) NOT NULL,
  `HospitalNotifiedDateTime` datetime(6) DEFAULT NULL,
  `ShiftType` int NOT NULL,
  `HospitalId` int NOT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  `BidEndDateTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `BidStartDateTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `MedicalUnitId` int NOT NULL DEFAULT '0',
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  KEY `IX_RotationSlots_HospitalId` (`HospitalId`),
  KEY `IX_RotationSlots_TenantId` (`TenantId`),
  KEY `IX_RotationSlots_MedicalUnitId` (`MedicalUnitId`),
  CONSTRAINT `FK_RotationSlots_Hospitals_HospitalId` FOREIGN KEY (`HospitalId`) REFERENCES `hospitals` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_RotationSlots_MedicalUnits_MedicalUnitId` FOREIGN KEY (`MedicalUnitId`) REFERENCES `medicalunits` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=23 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `slotavailabledays`
--

DROP TABLE IF EXISTS `slotavailabledays`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `slotavailabledays` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `TenantId` int DEFAULT NULL,
  `Day` int NOT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  `RotationSlotId` int DEFAULT NULL,
  `IsSelected` tinyint(1) NOT NULL DEFAULT '0',
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  KEY `IX_SlotAvailableDays_RotationSlotId` (`RotationSlotId`),
  CONSTRAINT `FK_SlotAvailableDays_RotationSlots_RotationSlotId` FOREIGN KEY (`RotationSlotId`) REFERENCES `rotationslots` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=190 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `slotrotationdays`
--

DROP TABLE IF EXISTS `slotrotationdays`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `slotrotationdays` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `TenantId` int DEFAULT NULL,
  `Day` int NOT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  `RotationSlotId` int DEFAULT NULL,
  `IsSelected` tinyint(1) NOT NULL DEFAULT '0',
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  KEY `IX_SlotRotationDays_RotationSlotId` (`RotationSlotId`),
  CONSTRAINT `FK_SlotRotationDays_RotationSlots_RotationSlotId` FOREIGN KEY (`RotationSlotId`) REFERENCES `rotationslots` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=190 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `surpathservices`
--

DROP TABLE IF EXISTS `surpathservices`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `surpathservices` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Price` double NOT NULL,
  `Discount` decimal(65,30) NOT NULL,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `TenantDepartmentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `CohortId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `UserId` bigint DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `IsEnabledByDefault` tinyint(1) NOT NULL DEFAULT '0',
  `FeatureIdentifier` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `RecordCategoryRuleId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_SurpathServices_CohortId` (`CohortId`),
  KEY `IX_SurpathServices_TenantDepartmentId` (`TenantDepartmentId`),
  KEY `IX_SurpathServices_TenantId` (`TenantId`),
  KEY `IX_SurpathServices_UserId` (`UserId`),
  KEY `IX_SurpathServices_RecordCategoryRuleId` (`RecordCategoryRuleId`),
  CONSTRAINT `FK_SurpathServices_AbpUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `abpusers` (`Id`),
  CONSTRAINT `FK_SurpathServices_Cohorts_CohortId` FOREIGN KEY (`CohortId`) REFERENCES `cohorts` (`Id`),
  CONSTRAINT `FK_SurpathServices_RecordCategoryRules_RecordCategoryRuleId` FOREIGN KEY (`RecordCategoryRuleId`) REFERENCES `recordcategoryrules` (`Id`),
  CONSTRAINT `FK_SurpathServices_TenantDepartments_TenantDepartmentId` FOREIGN KEY (`TenantDepartmentId`) REFERENCES `tenantdepartments` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tenantdepartmentorganizationunits`
--

DROP TABLE IF EXISTS `tenantdepartmentorganizationunits`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tenantdepartmentorganizationunits` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `TenantDepartmentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `OrganizationUnitId` bigint NOT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_TenantDepartmentOrganizationUnits_TenantId` (`TenantId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tenantdepartments`
--

DROP TABLE IF EXISTS `tenantdepartments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tenantdepartments` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Active` tinyint(1) NOT NULL,
  `MROType` int NOT NULL,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `CreationTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `CreatorUserId` bigint DEFAULT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  `OrganizationUnitId` bigint NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  KEY `IX_TenantDepartments_TenantId` (`TenantId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tenantdepartmentusers`
--

DROP TABLE IF EXISTS `tenantdepartmentusers`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tenantdepartmentusers` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `UserId` bigint NOT NULL,
  `TenantDepartmentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `CreationTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `CreatorUserId` bigint DEFAULT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_TenantDepartmentUsers_TenantDepartmentId` (`TenantDepartmentId`),
  KEY `IX_TenantDepartmentUsers_TenantId` (`TenantId`),
  KEY `IX_TenantDepartmentUsers_UserId` (`UserId`),
  CONSTRAINT `FK_TenantDepartmentUsers_AbpUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `abpusers` (`Id`) ON DELETE CASCADE,
  CONSTRAINT `FK_TenantDepartmentUsers_TenantDepartments_TenantDepartmentId` FOREIGN KEY (`TenantDepartmentId`) REFERENCES `tenantdepartments` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tenantdocumentcategories`
--

DROP TABLE IF EXISTS `tenantdocumentcategories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tenantdocumentcategories` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `AuthorizedOnly` tinyint(1) NOT NULL,
  `HostOnly` tinyint(1) NOT NULL,
  `UserId` bigint DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `CreatorUserId` bigint DEFAULT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_TenantDocumentCategories_UserId` (`UserId`),
  KEY `IX_TenantDocumentCategories_TenantId` (`TenantId`),
  CONSTRAINT `FK_TenantDocumentCategories_AbpUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `abpusers` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tenantdocuments`
--

DROP TABLE IF EXISTS `tenantdocuments`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tenantdocuments` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `AuthorizedOnly` tinyint(1) NOT NULL,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `TenantDocumentCategoryId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `RecordId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `CreatorUserId` bigint DEFAULT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_TenantDocuments_RecordId` (`RecordId`),
  KEY `IX_TenantDocuments_TenantDocumentCategoryId` (`TenantDocumentCategoryId`),
  KEY `IX_TenantDocuments_TenantId` (`TenantId`),
  CONSTRAINT `FK_TenantDocuments_Records_RecordId` FOREIGN KEY (`RecordId`) REFERENCES `records` (`Id`),
  CONSTRAINT `FK_TenantDocuments_TenantDocumentCategories_TenantDocumentCateg~` FOREIGN KEY (`TenantDocumentCategoryId`) REFERENCES `tenantdocumentcategories` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `tenantsurpathservices`
--

DROP TABLE IF EXISTS `tenantsurpathservices`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tenantsurpathservices` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `IsPricingOverrideEnabled` tinyint(1) NOT NULL,
  `SurpathServiceId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `RecordCategoryRuleId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `CohortId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Price` double NOT NULL DEFAULT '0',
  `TenantDepartmentId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `UserId` bigint DEFAULT NULL,
  `CohortUserId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `OrganizationUnitId` bigint DEFAULT NULL,
  `IsInvoiced` tinyint(1) NOT NULL DEFAULT '0',
  PRIMARY KEY (`Id`),
  KEY `IX_TenantSurpathServices_SurpathServiceId` (`SurpathServiceId`),
  KEY `IX_TenantSurpathServices_TenantId` (`TenantId`),
  KEY `IX_TenantSurpathServices_RecordCategoryRuleId` (`RecordCategoryRuleId`),
  KEY `IX_TenantSurpathServices_CohortId` (`CohortId`),
  KEY `IX_TenantSurpathServices_TenantDepartmentId` (`TenantDepartmentId`),
  KEY `IX_TenantSurpathServices_UserId` (`UserId`),
  KEY `IX_TenantSurpathServices_CohortUserId` (`CohortUserId`),
  KEY `IX_TenantSurpathServices_OrganizationUnitId` (`OrganizationUnitId`),
  CONSTRAINT `FK_TenantSurpathServices_AbpOrganizationUnits_OrganizationUnitId` FOREIGN KEY (`OrganizationUnitId`) REFERENCES `abporganizationunits` (`Id`),
  CONSTRAINT `FK_TenantSurpathServices_AbpUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `abpusers` (`Id`),
  CONSTRAINT `FK_TenantSurpathServices_Cohorts_CohortId` FOREIGN KEY (`CohortId`) REFERENCES `cohorts` (`Id`),
  CONSTRAINT `FK_TenantSurpathServices_CohortUsers_CohortUserId` FOREIGN KEY (`CohortUserId`) REFERENCES `cohortusers` (`Id`),
  CONSTRAINT `FK_TenantSurpathServices_RecordCategoryRules_RecordCategoryRule~` FOREIGN KEY (`RecordCategoryRuleId`) REFERENCES `recordcategoryrules` (`Id`),
  CONSTRAINT `FK_TenantSurpathServices_SurpathServices_SurpathServiceId` FOREIGN KEY (`SurpathServiceId`) REFERENCES `surpathservices` (`Id`),
  CONSTRAINT `FK_TenantSurpathServices_TenantDepartments_TenantDepartmentId` FOREIGN KEY (`TenantDepartmentId`) REFERENCES `tenantdepartments` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `testcategories`
--

DROP TABLE IF EXISTS `testcategories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `testcategories` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `Name` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `InternalName` varchar(120) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `CreationTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `CreatorUserId` bigint DEFAULT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `userpids`
--

DROP TABLE IF EXISTS `userpids`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `userpids` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `Pid` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Validated` tinyint(1) NOT NULL,
  `PidTypeId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `UserId` bigint DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00.000000',
  `CreatorUserId` bigint DEFAULT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL DEFAULT '0',
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_UserPids_PidTypeId` (`PidTypeId`),
  KEY `IX_UserPids_TenantId` (`TenantId`),
  KEY `IX_UserPids_UserId` (`UserId`),
  CONSTRAINT `FK_UserPids_AbpUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `abpusers` (`Id`),
  CONSTRAINT `FK_UserPids_PidTypes_PidTypeId` FOREIGN KEY (`PidTypeId`) REFERENCES `pidtypes` (`Id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `userpurchases`
--

DROP TABLE IF EXISTS `userpurchases`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `userpurchases` (
  `Id` char(36) CHARACTER SET ascii COLLATE ascii_general_ci NOT NULL,
  `TenantId` int DEFAULT NULL,
  `Name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `Status` int NOT NULL,
  `OriginalPrice` double NOT NULL,
  `AdjustedPrice` double NOT NULL,
  `DiscountAmount` double NOT NULL,
  `AmountPaid` double NOT NULL,
  `PaymentPeriodType` int NOT NULL,
  `PurchaseDate` datetime(6) NOT NULL,
  `ExpirationDate` datetime(6) DEFAULT NULL,
  `IsRecurring` tinyint(1) NOT NULL,
  `Notes` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `MetaData` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `UserId` bigint DEFAULT NULL,
  `SurpathServiceId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `TenantSurpathServiceId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `CohortId` char(36) CHARACTER SET ascii COLLATE ascii_general_ci DEFAULT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_UserPurchases_CohortId` (`CohortId`),
  KEY `IX_UserPurchases_SurpathServiceId` (`SurpathServiceId`),
  KEY `IX_UserPurchases_TenantId` (`TenantId`),
  KEY `IX_UserPurchases_TenantSurpathServiceId` (`TenantSurpathServiceId`),
  KEY `IX_UserPurchases_UserId` (`UserId`),
  CONSTRAINT `FK_UserPurchases_AbpUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `abpusers` (`Id`),
  CONSTRAINT `FK_UserPurchases_Cohorts_CohortId` FOREIGN KEY (`CohortId`) REFERENCES `cohorts` (`Id`),
  CONSTRAINT `FK_UserPurchases_SurpathServices_SurpathServiceId` FOREIGN KEY (`SurpathServiceId`) REFERENCES `surpathservices` (`Id`),
  CONSTRAINT `FK_UserPurchases_TenantSurpathServices_TenantSurpathServiceId` FOREIGN KEY (`TenantSurpathServiceId`) REFERENCES `tenantsurpathservices` (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `welcomemessages`
--

DROP TABLE IF EXISTS `welcomemessages`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `welcomemessages` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `TenantId` int DEFAULT NULL,
  `Title` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `Message` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `IsDefault` tinyint(1) NOT NULL,
  `DisplayStart` datetime(6) NOT NULL,
  `DisplayEnd` datetime(6) NOT NULL,
  `CreationTime` datetime(6) NOT NULL,
  `CreatorUserId` bigint DEFAULT NULL,
  `LastModificationTime` datetime(6) DEFAULT NULL,
  `LastModifierUserId` bigint DEFAULT NULL,
  `IsDeleted` tinyint(1) NOT NULL,
  `DeleterUserId` bigint DEFAULT NULL,
  `DeletionTime` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `IX_Welcomemessages_TenantId` (`TenantId`)
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping routines for database 'surpathv2'
--
/*!50003 DROP PROCEDURE IF EXISTS `OCCCleanup` */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'STRICT_TRANS_TABLES,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
CREATE DEFINER=`root`@`localhost` PROCEDURE `OCCCleanup`()
BEGIN
	DECLARE finished INTEGER DEFAULT 0;
    DECLARE tenantidval integer DEFAULT 0;
	

	
	DEClARE tenantidcur 
		CURSOR FOR 
			SELECT id FROM abptenants;

	
	DECLARE CONTINUE HANDLER 
        FOR NOT FOUND SET finished = 1;

	OPEN tenantidcur;

	fixOCCC: LOOP
		FETCH tenantidcur INTO tenantidval;
		IF finished = 1 THEN 
			LEAVE fixOCCC;
		END IF;


select tenantidval;

set @drugss = (select id from surpathservices where name like '%drug%');
set @backgroundss = (select id from surpathservices where name like '%background%');





set @bcid = (select id from recordrequirements where TenantId = tenantidval and IsSurpathOnly =1
and name like '%background%' and IsDeleted =0 and name not like '%OCCC%' limit 1);

set @dtid = (select id from recordrequirements where TenantId = tenantidval and IsSurpathOnly =1
and name like '%drug%' and IsDeleted =0 and name not like '%OCCC%' limit 1);



set @tenantdrugcatid = (select c.id from recordcategories c 
left outer join recordrequirements r on c.RecordRequirementId = r.id
where c.TenantId=tenantidval and c.IsDeleted = 0 and r.Id = @dtid and c.name like '%drug%' and c.name not like '%OCCC%' limit 1);

set @tenantbackgroundcatid = (select c.id from recordcategories c 
left outer join recordrequirements r on c.RecordRequirementId = r.id
where c.TenantId=tenantidval and c.IsDeleted = 0 and r.id = @bcid and c.name like '%background%' and c.name not like '%OCCC%' limit 1);



set @tenantssbackgroundid = (select t.id from tenantsurpathservices t
where t.TenantId = tenantidval and t.name like '%background%' and t.name not like '%OCCC%' and t.IsDeleted = 0);

set @tenantssdrugid = (select t.id from tenantsurpathservices t
where t.TenantId = tenantidval and t.name like '%drug%' and t.name not like '%OCCC%' and t.IsDeleted = 0);



update ledgerentrydetails l set l.TenantSurpathServiceId = @tenantssdrugid where l.SurpathServiceId = @drugss;
update ledgerentrydetails l set l.TenantSurpathServiceId = @tenantssbackgroundid where l.SurpathServiceId = @backgroundss;



update recordrequirements r set r.TenantSurpathServiceId = @tenantssdrugid 
where r.TenantId = tenantidval and r.SurpathServiceId = @drugss;
update recordrequirements r set r.TenantSurpathServiceId = @tenantssbackgroundid 
where r.TenantId = tenantidval and r.SurpathServiceId = @backgroundss;







update recordcategories c
set c.RecordRequirementId = @dtid
where c.TenantId = tenantidval and c.Name like '%Drug%';
update recordcategories c
set c.RecordRequirementId = @bcid
where c.TenantId = tenantidval and c.Name like '%Background%';

update recordcategories c 
set isdeleted = 1
where c.RecordRequirementId = @dtid and c.Id != @tenantdrugcatid and tenantid = tenantidval;
update recordcategories c 
set isdeleted = 1
where c.RecordRequirementId = @bcid and c.Id != @tenantbackgroundcatid and tenantid = tenantidval;




update recordstates rs
set RecordCategoryId = @tenantdrugcatid
where rs.TenantId = tenantidval and rs.RecordCategoryId in 
(
select c.id from recordcategories c 
left outer join recordrequirements r on c.RecordRequirementId = r.id
where c.TenantId=tenantidval and r.IsSurpathOnly = 1 and c.name like '%drug%' 
);
update recordstates rs
set RecordCategoryId = @tenantbackgroundcatid
where rs.TenantId = tenantidval and rs.RecordCategoryId in 
(
select c.id from recordcategories c 
left outer join recordrequirements r on c.RecordRequirementId = r.id
where c.TenantId=tenantidval and r.IsSurpathOnly = 1 and c.name like '%background%' 
);

update recordcategories c
left outer join recordrequirements r on c.RecordRequirementId = r.id
set c.isDeleted = 1
where c.TenantId = tenantidval and r.IsSurpathOnly = 1 and c.name like '%drug%' and r.id != @dtid;

update recordcategories c
left outer join recordrequirements r on c.RecordRequirementId = r.id
set c.isDeleted = 1
where c.TenantId = tenantidval and r.IsSurpathOnly = 1 and c.name like '%background%' and r.id != @bcid;





update tenantsurpathservices t
set t.IsDeleted = 1, t.isEnabled = 0
where t.TenantId = tenantidval and t.name like '%OCCC%';




update recordrequirements r 
set r.IsDeleted = 1
where r.TenantId=tenantidval and r.IsSurpathOnly = 1 and r.name like '%drug%' and r.id != @dtid;
update recordrequirements r 
set r.IsDeleted = 1
where r.TenantId=tenantidval and r.IsSurpathOnly = 1 and r.name like '%background%' and r.id != @bcid;





	END LOOP fixOCCC;
	CLOSE tenantidcur;

END ;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2025-09-18 12:54:38
