<?php

require_once 'monProg.php';
require_once 'uniter/uniter.php';

// Préparer les tests : titre, entrée, résultat attendu
$entrees = 
[
	new Entree('nbre = -1', -1, -2),
	new Entree('nbre = 0', 0, 0),
	new Entree('nbre = 2', 2, 4),
	new Entree('nbre = 10', 10, 200),
	new Entree('nbre = \'str\'', 'str', InvalidArgumentException::class),
	new Entree('nbre = true', true, InvalidArgumentException::class),
	new Entree('nbre = [\'nom\' => \'Toto\']', ['nom' => 'Toto'], InvalidArgumentException::class),
];

// La fonction à appeler
$nomFonction = 'MultiplierPar2';

// Fichier de sortie
$maintenant = date('Y_m_d_His');
$cheminFichier = "X:\\...\\...\\data_{$maintenant}.csv";

// Instancier Uniter
$uniter = new Uniter($cheminFichier, $nomFonction, $entrees);

// Lancer la procédure de test avec fichier CSV généré en sortie
// $uniter->TesterCSV();
// OU BIEN 
// Lancer la procédure de test avec renvoi des résultats en JSON
echo $uniter->TesterJSON();
