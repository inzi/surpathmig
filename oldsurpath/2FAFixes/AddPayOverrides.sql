DROP TABLE IF EXISTS surpathlive.overridedonorpay;

CREATE TABLE `overridedonorpay` (
  `TransID` varchar(32) DEFAULT NULL,
  `InvoiceNumber` varchar(20) DEFAULT NULL,
  `TransStatus` varchar(20) DEFAULT NULL,  
  `SubmitDate` datetime DEFAULT NULL,
  `LastName` varchar(128) DEFAULT NULL,
  `FirstName` varchar(128) DEFAULT NULL,
  `Phone` varchar(20) DEFAULT NULL,
  `Email` varchar(255) DEFAULT NULL,
  `Card` varchar(20) DEFAULT NULL,
  `PaymentMethod` varchar(128) DEFAULT NULL,
  `PaymentAmount` varchar(20) DEFAULT NULL,
  `SettlementDate` datetime DEFAULT NULL,
  `SettlementAmount` varchar(20) DEFAULT NULL,
  `Used` int(11) DEFAULT '0',
  `DateUsed` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;



INSERT INTO surpathlive.overridedonorpay(
   TransID
  ,InvoiceNumber
  ,TransStatus   
  ,SubmitDate
  ,LastName
  ,FirstName
  ,Phone
  ,Email
  ,Card
  ,PaymentMethod
  ,PaymentAmount
  ,SettlementDate
  ,SettlementAmount
  ,Used
  ,DateUsed
) VALUES 
('43271691477','None','Settled Successfully',STR_TO_DATE('3/14/22 16:07', '%m/%d/%Y %T'),'Alfaro','Alondra','2146796041','alondraalfaro127@gmail.com','V','XXXX2904','USD 105.17',STR_TO_DATE('3/15/22 15:28', '%m/%d/%Y %T'),'USD 105.17',0,NULL),
('43269677572','None','Settled Successfully',STR_TO_DATE('3/13/22 11:58', '%m/%d/%Y %T'),'Allen','Camden','9407832527','callen@littleelm.org','V','XXXX9089','USD 15.00',STR_TO_DATE('3/13/22 15:26', '%m/%d/%Y %T'),'USD 15.00',0,NULL),
('43273439732','None','Settled Successfully',STR_TO_DATE('3/15/22 13:08', '%m/%d/%Y %T'),'Alvarado','Paola','682-314-8821','pamers28@gmail.com','V','XXXX5690','USD 95.17',STR_TO_DATE('3/15/22 15:28', '%m/%d/%Y %T'),'USD 95.17',0,NULL),
('43269730761','None','Settled Successfully',STR_TO_DATE('3/13/22 12:38', '%m/%d/%Y %T'),'Augustine','Sierra','2282348875','sierra.augustine2013@gmail.com','V','XXXX4445','USD 105.17',STR_TO_DATE('3/13/22 15:26', '%m/%d/%Y %T'),'USD 105.17',0,NULL),
('43273412323','None','Settled Successfully',STR_TO_DATE('3/15/22 12:55', '%m/%d/%Y %T'),'Baker','Jennifer','4694062069','madib122105@gmail.com','M','XXXX5448','USD 70.00',STR_TO_DATE('3/15/22 15:28', '%m/%d/%Y %T'),'USD 70.00',0,NULL),
('43270845462','None','Settled Successfully',STR_TO_DATE('3/14/22 9:11', '%m/%d/%Y %T'),'Barreras','Wendy','2149168302','wendy_sbv@hotmail.com','V','XXXX7123','USD 105.17',STR_TO_DATE('3/14/22 15:34', '%m/%d/%Y %T'),'USD 105.17',0,NULL),
('43271056368','None','Settled Successfully',STR_TO_DATE('3/14/22 10:49', '%m/%d/%Y %T'),'Burrus','Khara','9515141237','e3274449@student.dcccd.edu','V','XXXX7525','USD 105.17',STR_TO_DATE('3/14/22 15:34', '%m/%d/%Y %T'),'USD 105.17',0,NULL),
('43268580420','None','Settled Successfully',STR_TO_DATE('3/12/22 15:01', '%m/%d/%Y %T'),'Cundieff','Alexandra','8176899446','alexcundieff@gmail.com','D','XXXX1833','USD 105.17',STR_TO_DATE('3/13/22 15:26', '%m/%d/%Y %T'),'USD 105.17',0,NULL),
('43269681102','None','Settled Successfully',STR_TO_DATE('3/13/22 12:00', '%m/%d/%Y %T'),'Espinoza','Yulma','2145975109','emilymcdonald1919@gmail.com','V','XXXX0250','USD 70.00',STR_TO_DATE('3/13/22 15:26', '%m/%d/%Y %T'),'USD 70.00',0,NULL),
('43271660200','None','Settled Successfully',STR_TO_DATE('3/14/22 15:48', '%m/%d/%Y %T'),'Frasure','vane','2146432376','vanessamakini@yahoo.com','V','XXXX2323','USD 105.17',STR_TO_DATE('3/15/22 15:28', '%m/%d/%Y %T'),'USD 105.17',0,NULL),
('43271812280','None','Settled Successfully',STR_TO_DATE('3/14/22 17:29', '%m/%d/%Y %T'),'gonzalez pineda','Karen','2144517162','Karenrubyg@gmail.com','V','XXXX2508','USD 105.17',STR_TO_DATE('3/15/22 15:28', '%m/%d/%Y %T'),'USD 105.17',0,NULL),
('43268698639','None','Settled Successfully',STR_TO_DATE('3/12/22 16:18', '%m/%d/%Y %T'),'Kaur','Jatinder','3462180397','taniyasudan123@gmail.com','A','XXXX5638','USD 45.32',STR_TO_DATE('3/13/22 15:26', '%m/%d/%Y %T'),'USD 45.32',0,NULL),
('43272978029','None','Settled Successfully',STR_TO_DATE('3/15/22 9:47', '%m/%d/%Y %T'),'Kirby','Brian','4697894216','brian.kirby.w@gmail.com','M','XXXX6893','USD 105.17',STR_TO_DATE('3/15/22 15:28', '%m/%d/%Y %T'),'USD 105.17',0,NULL),
('43268792036','None','Settled Successfully',STR_TO_DATE('3/12/22 17:20', '%m/%d/%Y %T'),'Mengistu','Firehiwot','913-299-7017','firehiwotsenay@yahoo.com','M','XXXX5528','USD 105.17',STR_TO_DATE('3/13/22 15:26', '%m/%d/%Y %T'),'USD 105.17',0,NULL),
('43271887054','None','Settled Successfully',STR_TO_DATE('3/14/22 18:25', '%m/%d/%Y %T'),'Metersky','Yana','4699542539','zoeymetersky@gmail.com','V','XXXX0719','USD 70.00',STR_TO_DATE('3/15/22 15:28', '%m/%d/%Y %T'),'USD 70.00',0,NULL),
('43271976787','None','Settled Successfully',STR_TO_DATE('3/14/22 19:49', '%m/%d/%Y %T'),'Ndakwah','Belinda','469-285-5609','Belindakeh@gmail.com','M','XXXX9264','USD 105.17',STR_TO_DATE('3/15/22 15:28', '%m/%d/%Y %T'),'USD 105.17',0,NULL),
('43271898152','None','Settled Successfully',STR_TO_DATE('3/14/22 18:34', '%m/%d/%Y %T'),'Niemann','Thomas','2812549713','tomniemann382@gmail.com','A','XXXX5078','USD 45.32',STR_TO_DATE('3/15/22 15:28', '%m/%d/%Y %T'),'USD 45.32',0,NULL),
('43271193490','None','Settled Successfully',STR_TO_DATE('3/14/22 11:50', '%m/%d/%Y %T'),'Owunna','Grace','3477014412','Gowunna096@gmail.com','M','XXXX9630','USD 105.17',STR_TO_DATE('3/14/22 15:34', '%m/%d/%Y %T'),'USD 105.17',0,NULL),
('43270462332','None','Settled Successfully',STR_TO_DATE('3/14/22 5:36', '%m/%d/%Y %T'),'Paras','Lisa','8172170584','lpheavenly@yahoo.com','V','XXXX3861','USD 105.17',STR_TO_DATE('3/14/22 15:34', '%m/%d/%Y %T'),'USD 105.17',0,NULL),
('43270127779','None','Settled Successfully',STR_TO_DATE('3/13/22 17:27', '%m/%d/%Y %T'),'Paul','Meghna','832-907-4411','pauliose2000@gmail.com','V','XXXX9935','USD 45.32',STR_TO_DATE('3/14/22 15:34', '%m/%d/%Y %T'),'USD 45.32',0,NULL),
('43270147348','None','Settled Successfully',STR_TO_DATE('3/13/22 17:44', '%m/%d/%Y %T'),'Ridley','Darren','3253709408','djridley11@gmail.com','M','XXXX0240','USD 45.32',STR_TO_DATE('3/14/22 15:34', '%m/%d/%Y %T'),'USD 45.32',0,NULL),
('43272861834','None','Settled Successfully',STR_TO_DATE('3/15/22 8:59', '%m/%d/%Y %T'),'Rodriguez','Cristina','4695311250','Heidirodriguez45@yahoo.com','M','XXXX3089','USD 105.17',STR_TO_DATE('3/15/22 15:28', '%m/%d/%Y %T'),'USD 105.17',0,NULL),
('63593526488','None','Settled Successfully',STR_TO_DATE('3/13/22 20:31', '%m/%d/%Y %T'),'Rodriguez','Luis','8327985459','lrod13443@gmail.com','V','XXXX6255','USD 45.32',STR_TO_DATE('3/14/22 15:34', '%m/%d/%Y %T'),'USD 45.32',0,NULL),
('43271180571','None','Settled Successfully',STR_TO_DATE('3/14/22 11:44', '%m/%d/%Y %T'),'Tekleab','Senay','4694030983','senaysolomon305@gmail.com','M','XXXX7206','USD 105.17',STR_TO_DATE('3/14/22 15:34', '%m/%d/%Y %T'),'USD 105.17',0,NULL),
('43267213428','None','Settled Successfully',STR_TO_DATE('3/11/22 20:04', '%m/%d/%Y %T'),'tran','grace','2816248349','ghtran2@gmail.com','V','XXXX0783','USD 45.32',STR_TO_DATE('3/12/22 15:27', '%m/%d/%Y %T'),'USD 45.32',0,NULL),
('43271983468','None','Settled Successfully',STR_TO_DATE('3/14/22 19:57', '%m/%d/%Y %T'),'Williams','Norika','9012379703','norikaswilliams@gmail.com','D','XXXX8446','USD 105.17',STR_TO_DATE('3/15/22 15:28', '%m/%d/%Y %T'),'USD 105.17',0,NULL),
('43270219267','None','Settled Successfully',STR_TO_DATE('3/13/22 18:55', '%m/%d/%Y %T'),'woldemeskel','samerawit','2149624316','samemulatu@yahoo.com','V','XXXX9963','USD 80.17',STR_TO_DATE('3/14/22 15:34', '%m/%d/%Y %T'),'USD 80.17',0,NULL),
('43266570703','None','Settled Successfully',STR_TO_DATE('3/11/22 14:20', '%m/%d/%Y %T'),'Alvarado','Gina','4696500384','Gina12367809a@gmail.com','V','XXXX8896','USD 105.17',STR_TO_DATE('3/11/22 15:29', '%m/%d/%Y %T'),'USD 105.17',0,NULL),
('43266127779','None','Settled Successfully',STR_TO_DATE('3/11/22 10:47', '%m/%d/%Y %T'),'Dew','Antandra','2144907710','antandradew@gmail.com','V','XXXX8749','USD 95.17',STR_TO_DATE('3/11/22 15:29', '%m/%d/%Y %T'),'USD 95.17',0,NULL),
('43268515200','None','Settled Successfully',STR_TO_DATE('3/12/22 14:15', '%m/%d/%Y %T'),'Reardon','Elena','Not Posted','reardon.elena@gmail.com','V','XXXX5874','USD 45.32',STR_TO_DATE('3/12/22 15:27', '%m/%d/%Y %T'),'USD 45.32',0,NULL),
('43267232551','None','Settled Successfully',STR_TO_DATE('3/11/22 20:15', '%m/%d/%Y %T'),'Sepe','Jack Russel','9144415080','jackrs1201@gmail.com','V','XXXX8243','USD 45.32',STR_TO_DATE('3/12/22 15:27', '%m/%d/%Y %T'),'USD 45.32',0,NULL),
('43273352030','None','Settled Successfully',STR_TO_DATE('3/15/22 12:27', '%m/%d/%Y %T'),'Vargas','Gloria','2146201286','gvargas0805@yahoo.com','V','XXXX7519','USD 105.17',STR_TO_DATE('3/15/22 15:28', '%m/%d/%Y %T'),'USD 105.17',0,NULL),
('43264909620','None','Settled Successfully',STR_TO_DATE('3/10/22 17:17', '%m/%d/%Y %T'),'Vaughn','Bethany','4694272874','vaughn_bethany@yahoo.com','M','XXXX2864','USD 105.17',STR_TO_DATE('3/11/22 15:29', '%m/%d/%Y %T'),'USD 105.17',0,NULL)
;
-- SELECT STR_TO_DATE('2012-11-29 18:21:11.123', '%Y-%m-%d %T.%f'); 
-- SELECT STR_TO_DATE('3/14/22 16:07', '%m/%d/%Y %T'); 
select * from overridedonorpay;