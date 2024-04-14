<?php

require_once 'entree.php';

class Uniter
{
	// Champs

	private $CheminFichierSortie;
	private $nomFonction;
	private $entrees;
	private $enTete = ['ENTREE', 'SORTIE', 'ATTENDU', 'STATUT', 'MESSAGE'];
	private $resultats;

	// Constructeurs
	
	public function __construct($CheminFichierSortie, $nomFonction, $entrees)
	{
		$this->CheminFichierSortie = $CheminFichierSortie;
		$this->nomFonction = $nomFonction;
		$this->entrees = $entrees;
	}

	// Méthodes

	private function Demarrer()
	{
		$this->resultats = [];
		$this->resultats[] = $this->enTete;

		foreach ($this->entrees as $valeur) 
		{
			$resultat;
			$statut;
			$message = '';
			$resultatAttendu = $valeur->ResultatAttendu();

			try 
			{
				$resultat = call_user_func($this->nomFonction, $valeur->Entree());
			} 
			catch (Exception $e) 
			{
				$message = "{$resultatAttendu} : {$e->getMessage()}";
				$resultat = get_class($e);
			}
			finally
			{
				if($resultatAttendu === $resultat)
				{
					$statut = 'OK';
				}
				else
				{
					$statut = 'KO';
				}
			
				$titre = $valeur->Titre();

				// strval() pour obtenir des chaînes de caractères en sortie
				$this->resultats[] = [strval($titre), strval($resultat), strval($resultatAttendu), $statut, strval($message)];
			}
		}	
	}

	public function TesterJSON()
	{
		$this->Demarrer();

		$sortie = 
		[
			'CheminFichierSortie' => $this->CheminFichierSortie,
			'Resultats' => $this->resultats,
		];

		header('Content-Type: application/json');
		return json_encode($sortie);
	}

	public function TesterCSV()
	{
		$this->Demarrer();

		$fichierCSV = fopen($this->CheminFichierSortie, 'w');

		foreach ($this->resultats as $ligne) 
		{
			fputcsv($fichierCSV, $ligne, ";");
		}

		fclose($fichierCSV);
	}

}
