# Méthodologie

## Observation/Analyse
A la lecture de l'énoncé et du programme figurant en solution d'exemple, je constate que ce qui m'est demandé est de reprendre le malware généré en exercice 2 et de rendre le processus cible configurable en :  
- Prenant le nom du processus normalisé (en retirant le ".exe") en argument depuis la ligne de commande pour le rechercher
- Si le processus existe, prendre son PID directement
- Si aucun processus ne correspond au nom demandé, lancer le binaire correspondant et récupérer son PID
- Utiliser le bloc injection de l'exercice 2 mais en visant le PID obtenu

