-- phpMyAdmin SQL Dump
-- version 4.9.0.1
-- https://www.phpmyadmin.net/
--
-- Hôte : sql106.infinityfree.com
-- Généré le :  jeu. 07 mars 2024 à 10:01
-- Version du serveur :  10.4.17-MariaDB
-- Version de PHP :  7.2.22

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de données :  `if0_35172802_biblio`
--

-- --------------------------------------------------------

--
-- Structure de la table `auteur`
--

CREATE TABLE `auteur` (
  `id_auteur` int(11) NOT NULL,
  `nom` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `prenom` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Déchargement des données de la table `auteur`
--

INSERT INTO `auteur` (`id_auteur`, `nom`, `prenom`) VALUES
(1, 'Darwin', 'Charles Robert'),
(2, 'Doyle', 'Arthur Conan'),
(3, 'Hugo', 'Victor');

-- --------------------------------------------------------

--
-- Structure de la table `commentaire`
--

CREATE TABLE `commentaire` (
  `id_livre` int(11) NOT NULL,
  `id_user` int(11) NOT NULL,
  `id_com` int(11) NOT NULL,
  `commentaire` varchar(255) COLLATE utf8mb4_unicode_ci NOT NULL,
  `note` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `date_com` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Déchargement des données de la table `commentaire`
--

INSERT INTO `commentaire` (`id_livre`, `id_user`, `id_com`, `commentaire`, `note`, `date_com`) VALUES
(2, 7, 1, 'nice', '3', '2023-10-05'),
(3, 7, 2, 'beau', '4', '2023-10-05'),
(3, 7, 3, 'super', '1', '2023-10-05'),
(3, 7, 4, 'horrible', '5', '2023-10-05'),
(1, 7, 5, 'très misérable', '5', '2023-10-09'),
(1, 7, 7, 'chic', '1', '2023-10-12'),
(4, 7, 8, 'J\'aime', '5', '2023-10-19'),
(2, 16, 9, 'fascinant', '3', '2023-10-19'),
(4, 6, 12, 'Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor i', '5', '2023-10-21'),
(2, 17, 13, 'Enigme de qualité', '5', '2023-10-25');

-- --------------------------------------------------------

--
-- Structure de la table `emprunt`
--

CREATE TABLE `emprunt` (
  `id_exemplaire` int(11) NOT NULL,
  `id_user` int(11) NOT NULL,
  `id_emprunt` int(11) NOT NULL,
  `date_emprunt` date NOT NULL,
  `date_retour` date NOT NULL,
  `statut` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Déchargement des données de la table `emprunt`
--

INSERT INTO `emprunt` (`id_exemplaire`, `id_user`, `id_emprunt`, `date_emprunt`, `date_retour`, `statut`) VALUES
(1, 16, 55, '2023-10-28', '2023-10-30', 0),
(2, 6, 56, '2023-10-27', '2023-11-05', 0),
(3, 6, 57, '2023-11-05', '2023-12-07', 0),
(2, 6, 58, '2023-10-25', '2023-10-26', 0),
(3, 17, 59, '2023-10-27', '2023-10-31', 0),
(4, 17, 60, '2023-10-28', '2023-10-31', 0),
(1, 7, 61, '2023-10-26', '2023-10-28', 0),
(3, 17, 62, '2023-11-02', '2023-11-10', 0),
(2, 17, 63, '2023-11-02', '2023-12-01', 1),
(4, 17, 64, '2023-12-02', '2024-01-07', 0),
(4, 17, 65, '2023-11-02', '2023-12-02', 0);

-- --------------------------------------------------------

--
-- Structure de la table `exemplaire`
--

CREATE TABLE `exemplaire` (
  `id_exemplaire` int(11) NOT NULL,
  `disponible` tinyint(1) NOT NULL,
  `id_livre` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Déchargement des données de la table `exemplaire`
--

INSERT INTO `exemplaire` (`id_exemplaire`, `disponible`, `id_livre`) VALUES
(1, 1, 1),
(2, 0, 4),
(3, 1, 3),
(4, 1, 2),
(5, 1, 4);

-- --------------------------------------------------------

--
-- Structure de la table `livre`
--

CREATE TABLE `livre` (
  `id_livre` int(11) NOT NULL,
  `titre` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `image` varchar(255) COLLATE utf8mb4_unicode_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Déchargement des données de la table `livre`
--

INSERT INTO `livre` (`id_livre`, `titre`, `image`) VALUES
(1, 'Les misérables', '../images/misere.jpg'),
(2, 'Le chien des Baskerville', '../images/chien.jpg'),
(3, 'La vallée de la peur', '../images/vallee.jpg'),
(4, 'L\'origine des espèces', '../images/origine.jpg');

-- --------------------------------------------------------

--
-- Structure de la table `oeuvre_de`
--

CREATE TABLE `oeuvre_de` (
  `id_livre` int(11) NOT NULL,
  `id_auteur` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Déchargement des données de la table `oeuvre_de`
--

INSERT INTO `oeuvre_de` (`id_livre`, `id_auteur`) VALUES
(1, 3),
(2, 2),
(3, 2),
(4, 1);

-- --------------------------------------------------------

--
-- Structure de la table `user`
--

CREATE TABLE `user` (
  `id_user` int(11) NOT NULL,
  `nom` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `prenom` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `email` varchar(50) COLLATE utf8mb4_unicode_ci NOT NULL,
  `motdepasse` varchar(1000) COLLATE utf8mb4_unicode_ci NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Déchargement des données de la table `user`
--

INSERT INTO `user` (`id_user`, `nom`, `prenom`, `email`, `motdepasse`) VALUES
(5, 'ee', 'ee', 'e@e.e', '$2y$10$.OPxUYjlscxVhJgww7ZdLOhyV0iQAkUrLfcuiJG8jTzupMId1YzZ2'),
(6, 'xx', 'xx', 'x@x.x', '$2y$10$aitOa21ORwz4PPq3f2pXUuxwUbQt.6DghymR9SWklpCfk71R3AVHi'),
(7, 'Jack', 'Chirac', 'chirac@c.fr', '$2y$10$3MnteZhPSsZgU9bf41PPUujEcz93P1jje84VRhponRMYNt9s96Uw2'),
(8, 'DA SILVA', 'WILLIAM', 'WILLIAM@HOTMAIL.COM', '$2y$10$MD6wRfS7SI9F6htpQyqSJ.3i7AW8l9RpfFq8ZlUhFNHITl9brOZCm'),
(9, '22222', '1111', '11@gmail', '$2y$10$ePqmuqm9oai2QGjXahv8o.OXPxokLDTJQMbUu0fLUXH7Wzb7u9RCq'),
(16, 'c', 'c', 'c@c.c', '$2y$10$wOfwnkMOp6aaxvt7dvKUZOarx1jRMhf9lLPlEQ014cSQFocFsF5KG'),
(17, 'Smith', 'John', 'john@smith.fr', '$2y$10$97KbiW77SMe.npxRtBv2OOcQ8t22rCDMP824Qp9fVDjwS8EeU8Bsa'),
(18, 'John', 'Snow', 'j@s.js', '$2y$10$Os.K9251n1bGfvTybk7MDOoLH77yqsTsZ3cADAtR8wPTVWPby86aG');

--
-- Index pour les tables déchargées
--

--
-- Index pour la table `auteur`
--
ALTER TABLE `auteur`
  ADD PRIMARY KEY (`id_auteur`);

--
-- Index pour la table `commentaire`
--
ALTER TABLE `commentaire`
  ADD PRIMARY KEY (`id_com`),
  ADD KEY `id_livre` (`id_livre`),
  ADD KEY `id_user` (`id_user`);

--
-- Index pour la table `emprunt`
--
ALTER TABLE `emprunt`
  ADD PRIMARY KEY (`id_emprunt`),
  ADD KEY `id_exemplaire` (`id_exemplaire`),
  ADD KEY `id_user` (`id_user`);

--
-- Index pour la table `exemplaire`
--
ALTER TABLE `exemplaire`
  ADD PRIMARY KEY (`id_exemplaire`),
  ADD KEY `Exemplaire_Livre_FK` (`id_livre`);

--
-- Index pour la table `livre`
--
ALTER TABLE `livre`
  ADD PRIMARY KEY (`id_livre`);

--
-- Index pour la table `oeuvre_de`
--
ALTER TABLE `oeuvre_de`
  ADD PRIMARY KEY (`id_livre`,`id_auteur`),
  ADD KEY `oeuvre_de_Auteur0_FK` (`id_auteur`);

--
-- Index pour la table `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`id_user`);

--
-- AUTO_INCREMENT pour les tables déchargées
--

--
-- AUTO_INCREMENT pour la table `auteur`
--
ALTER TABLE `auteur`
  MODIFY `id_auteur` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT pour la table `commentaire`
--
ALTER TABLE `commentaire`
  MODIFY `id_com` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- AUTO_INCREMENT pour la table `emprunt`
--
ALTER TABLE `emprunt`
  MODIFY `id_emprunt` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=66;

--
-- AUTO_INCREMENT pour la table `exemplaire`
--
ALTER TABLE `exemplaire`
  MODIFY `id_exemplaire` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT pour la table `livre`
--
ALTER TABLE `livre`
  MODIFY `id_livre` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT pour la table `user`
--
ALTER TABLE `user`
  MODIFY `id_user` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=19;

--
-- Contraintes pour les tables déchargées
--

--
-- Contraintes pour la table `commentaire`
--
ALTER TABLE `commentaire`
  ADD CONSTRAINT `Commentaire_Livre_FK` FOREIGN KEY (`id_livre`) REFERENCES `livre` (`id_livre`),
  ADD CONSTRAINT `Commentaire_Utilisateur0_FK` FOREIGN KEY (`id_user`) REFERENCES `user` (`id_user`);

--
-- Contraintes pour la table `emprunt`
--
ALTER TABLE `emprunt`
  ADD CONSTRAINT `Emprunt_Exemplaire_FK` FOREIGN KEY (`id_exemplaire`) REFERENCES `exemplaire` (`id_exemplaire`),
  ADD CONSTRAINT `Emprunt_Utilisateur0_FK` FOREIGN KEY (`id_user`) REFERENCES `user` (`id_user`);

--
-- Contraintes pour la table `exemplaire`
--
ALTER TABLE `exemplaire`
  ADD CONSTRAINT `Exemplaire_Livre_FK` FOREIGN KEY (`id_livre`) REFERENCES `livre` (`id_livre`);

--
-- Contraintes pour la table `oeuvre_de`
--
ALTER TABLE `oeuvre_de`
  ADD CONSTRAINT `oeuvre_de_Auteur0_FK` FOREIGN KEY (`id_auteur`) REFERENCES `auteur` (`id_auteur`),
  ADD CONSTRAINT `oeuvre_de_Livre_FK` FOREIGN KEY (`id_livre`) REFERENCES `livre` (`id_livre`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
