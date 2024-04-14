<?php

function MultiplierPar2($nbre)
{
	if(!is_numeric($nbre)) 
	{
		throw new InvalidArgumentException('nbre n\'est pas un nombre.');
	}

	// Anomalie
	if($nbre == 10)
	{
		return -9.99;
	}

	return $nbre * 2;
}

// $entree = 2;
// $resultat = MultiplierPar2(2); // 4
