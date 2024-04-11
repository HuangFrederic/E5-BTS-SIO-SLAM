-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Hôte : 127.0.0.1
-- Généré le : jeu. 11 avr. 2024 à 10:47
-- Version du serveur : 8.0.20
-- Version de PHP : 8.2.9

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de données : `dvd`
--

-- --------------------------------------------------------

--
-- Structure de la table `client`
--

CREATE TABLE `client` (
  `ClientId` int NOT NULL,
  `Nom` varchar(50) NOT NULL,
  `Prenom` varchar(50) NOT NULL,
  `Adresse` varchar(100) NOT NULL,
  `NumTel` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

--
-- Déchargement des données de la table `client`
--

INSERT INTO `client` (`ClientId`, `Nom`, `Prenom`, `Adresse`, `NumTel`) VALUES
(1, 'Paul', 'Jean', 'a', '8'),
(2, 'b', 'b', 'b', '9'),
(3, 'Sand', 'John', 'z', '01 02 03 04 05'),
(8, 'Long', 'Law', 'b', '11 22 33 44 55');

-- --------------------------------------------------------

--
-- Structure de la table `dvd`
--

CREATE TABLE `dvd` (
  `DVDId` int NOT NULL,
  `Title` varchar(100) NOT NULL,
  `Director` varchar(50) NOT NULL,
  `Genre` varchar(50) NOT NULL,
  `ReleaseYear` int NOT NULL,
  `IsAvailable` int NOT NULL,
  `Image` varchar(250) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 ROW_FORMAT=COMPACT;

--
-- Déchargement des données de la table `dvd`
--

INSERT INTO `dvd` (`DVDId`, `Title`, `Director`, `Genre`, `ReleaseYear`, `IsAvailable`, `Image`) VALUES
(1, 'Le Seigneur des Anneaux La Communauté de l\'Anneaux', 'Peter Jackson', 'Fiction', 2001, 0, 'anneaux1.jpg'),
(2, 'Le Seigneur des Anneaux : Les Deux Tours', 'Peter Jackson', 'Fiction', 2002, 0, 'anneaux2.jpg'),
(3, 'Le Seigneur des Anneaux : Le Retour du Roi', 'Peter Jackson', 'Fiction', 2003, 0, 'anneaux3.jpg'),
(4, 'r', 'r', 'Drama', 2000, 1, 'misere.jpg'),
(9, 'a', 'a', 'Fantasy', 1999, 1, 'chien.jpg');

-- --------------------------------------------------------

--
-- Structure de la table `location`
--

CREATE TABLE `location` (
  `LocationId` int NOT NULL,
  `LeClient` int NOT NULL,
  `LeDVD` int NOT NULL,
  `dateRented` date NOT NULL,
  `dateReturned` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Déchargement des données de la table `location`
--

INSERT INTO `location` (`LocationId`, `LeClient`, `LeDVD`, `dateRented`, `dateReturned`) VALUES
(12, 8, 2, '2024-02-14', '2024-03-01'),
(13, 1, 3, '2024-02-15', '2024-02-15'),
(19, 8, 1, '2024-03-16', '2024-03-31');

--
-- Déclencheurs `location`
--
DELIMITER $$
CREATE TRIGGER `addNewRetour` AFTER INSERT ON `location` FOR EACH ROW BEGIN
    DECLARE LocationPrix DECIMAL(10, 2);
    
    -- Définir le prix fixe à 10€
    SET LocationPrix = 10.00;
    
    -- Insérer dans la table `retour`
    INSERT INTO retour (LaLocation, DateReturned, LocationPrix, Retourner)
    VALUES (NEW.LocationId, NEW.dateReturned, LocationPrix, 0);
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `afterInsertLocationRapport` AFTER INSERT ON `location` FOR EACH ROW BEGIN
    DECLARE ClientName VARCHAR(50);
    DECLARE DVDTitle VARCHAR(100);
    DECLARE ContentText VARCHAR(255);
    
    SELECT Nom INTO ClientName FROM client WHERE ClientId = NEW.LeClient;
    
    SELECT Title INTO DVDTitle FROM dvd WHERE DVDId = NEW.LeDVD;
    
    SET ContentText = CONCAT('Le client ', ClientName, ' a loué le DVD ', DVDTitle, 
                             ' le ', NEW.dateRented);
    
    INSERT INTO rapport (DateGenerated, Content) VALUES (NOW(), ContentText);
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `afterUpdateLocation` AFTER UPDATE ON `location` FOR EACH ROW BEGIN
    -- Si la date de retour a été modifiée, mettre à jour la table `retour`
    IF NEW.dateReturned <> OLD.dateReturned THEN
        UPDATE retour SET DateReturned = NEW.dateReturned WHERE LaLocation = NEW.LocationId;
    END IF;
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `beforeDeleteLocationForRetour` BEFORE DELETE ON `location` FOR EACH ROW BEGIN
    -- Mettre à jour le statut IsAvailable du DVD associé à la location
    UPDATE dvd SET IsAvailable = 1 WHERE DVDId = OLD.LeDVD;
    
    -- Supprimer les enregistrements liés dans la table `retour`
    DELETE FROM retour WHERE LaLocation = OLD.LocationId;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Structure de la table `rapport`
--

CREATE TABLE `rapport` (
  `RapportId` int NOT NULL,
  `DateGenerated` datetime NOT NULL,
  `Content` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Déchargement des données de la table `rapport`
--

INSERT INTO `rapport` (`RapportId`, `DateGenerated`, `Content`) VALUES
(2, '2024-02-29 09:57:07', 'Le client Long a loué le DVD Le Seigneur des Anneaux : Le Retour du Roi le 2024-02-29'),
(3, '2024-02-29 09:57:20', 'Le client Long a annulé le retour du DVD Le Seigneur des Anneaux : Le Retour du Roi'),
(4, '2024-02-29 16:06:30', 'Le client Long a loué le DVD Le Seigneur des Anneaux : Le Retour du Roi le 2024-02-29'),
(5, '2024-02-29 16:08:03', 'Le client Long a annulé le retour du DVD Le Seigneur des Anneaux : Le Retour du Roi'),
(6, '2024-03-16 21:31:51', 'Le client Long a loué le DVD Le Seigneur des Anneaux La Communauté de l\'Anneaux le 2024-03-16');

-- --------------------------------------------------------

--
-- Structure de la table `retour`
--

CREATE TABLE `retour` (
  `RetourId` int NOT NULL,
  `LaLocation` int NOT NULL,
  `DateReturned` date NOT NULL,
  `LocationPrix` decimal(11,0) NOT NULL,
  `Retourner` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Déchargement des données de la table `retour`
--

INSERT INTO `retour` (`RetourId`, `LaLocation`, `DateReturned`, `LocationPrix`, `Retourner`) VALUES
(8, 12, '2024-03-01', 50, 0),
(9, 13, '2024-02-15', 150, 0),
(15, 19, '2024-03-31', 20, 0);

--
-- Déclencheurs `retour`
--
DELIMITER $$
CREATE TRIGGER `afterDeleteRetourRapport` AFTER DELETE ON `retour` FOR EACH ROW BEGIN
    DECLARE ClientName VARCHAR(50);
    DECLARE DVDTitle VARCHAR(100);
    DECLARE ContentText VARCHAR(255);
    
    SELECT Nom INTO ClientName FROM client WHERE ClientId = (SELECT LeClient FROM location WHERE LocationId = OLD.LaLocation);
    
    SELECT Title INTO DVDTitle FROM dvd WHERE DVDId = (SELECT LeDVD FROM location WHERE LocationId = OLD.LaLocation);
    
    SET ContentText = CONCAT('Le client ', ClientName, ' a annulé le retour du DVD ', DVDTitle);
    
    INSERT INTO rapport (DateGenerated, Content) VALUES (NOW(), ContentText);
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Structure de la table `user`
--

CREATE TABLE `user` (
  `UserId` int NOT NULL,
  `Username` varchar(50) NOT NULL,
  `Password` varchar(150) NOT NULL,
  `IsAdmin` int NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Déchargement des données de la table `user`
--

INSERT INTO `user` (`UserId`, `Username`, `Password`, `IsAdmin`) VALUES
(1, 'a', 'a', 1),
(2, 'b', 'zFCLc34t', 0),
(3, 'c', 'w0z5CY9', 0);

--
-- Index pour les tables déchargées
--

--
-- Index pour la table `client`
--
ALTER TABLE `client`
  ADD PRIMARY KEY (`ClientId`);

--
-- Index pour la table `dvd`
--
ALTER TABLE `dvd`
  ADD PRIMARY KEY (`DVDId`);

--
-- Index pour la table `location`
--
ALTER TABLE `location`
  ADD PRIMARY KEY (`LocationId`),
  ADD KEY `LeClient` (`LeClient`),
  ADD KEY `LeDVD` (`LeDVD`);

--
-- Index pour la table `rapport`
--
ALTER TABLE `rapport`
  ADD PRIMARY KEY (`RapportId`);

--
-- Index pour la table `retour`
--
ALTER TABLE `retour`
  ADD PRIMARY KEY (`RetourId`),
  ADD KEY `LaLocation` (`LaLocation`);

--
-- Index pour la table `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`UserId`);

--
-- AUTO_INCREMENT pour les tables déchargées
--

--
-- AUTO_INCREMENT pour la table `client`
--
ALTER TABLE `client`
  MODIFY `ClientId` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT pour la table `dvd`
--
ALTER TABLE `dvd`
  MODIFY `DVDId` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT pour la table `location`
--
ALTER TABLE `location`
  MODIFY `LocationId` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=20;

--
-- AUTO_INCREMENT pour la table `rapport`
--
ALTER TABLE `rapport`
  MODIFY `RapportId` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT pour la table `retour`
--
ALTER TABLE `retour`
  MODIFY `RetourId` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=16;

--
-- AUTO_INCREMENT pour la table `user`
--
ALTER TABLE `user`
  MODIFY `UserId` int NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- Contraintes pour les tables déchargées
--

--
-- Contraintes pour la table `location`
--
ALTER TABLE `location`
  ADD CONSTRAINT `location_ibfk_1` FOREIGN KEY (`LeClient`) REFERENCES `client` (`ClientId`),
  ADD CONSTRAINT `location_ibfk_2` FOREIGN KEY (`LeDVD`) REFERENCES `dvd` (`DVDId`);

--
-- Contraintes pour la table `retour`
--
ALTER TABLE `retour`
  ADD CONSTRAINT `retour_ibfk_1` FOREIGN KEY (`LaLocation`) REFERENCES `location` (`LocationId`);

DELIMITER $$
--
-- Évènements
--
CREATE DEFINER=`root`@`localhost` EVENT `AddLateFee` ON SCHEDULE EVERY 1 DAY STARTS '2024-02-15 09:35:00' ON COMPLETION NOT PRESERVE ENABLE DO UPDATE retour
SET LocationPrix = LocationPrix + 10
WHERE DateReturned < CURDATE() AND Retourner = 0$$

DELIMITER ;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
