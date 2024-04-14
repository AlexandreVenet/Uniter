<?php

class Entree
{
	private $titre;
	private $entree;
	private $resultatAttendu;

	public function __construct($titre, $entree, $resultatAttendu)
	{
		$this->titre = $titre;
		$this->entree = $entree;
		$this->resultatAttendu = $resultatAttendu;
	}

	public function Titre() { return $this->titre; }
	public function Entree() { return $this->entree; }
	public function ResultatAttendu() { return $this->resultatAttendu; }
}
