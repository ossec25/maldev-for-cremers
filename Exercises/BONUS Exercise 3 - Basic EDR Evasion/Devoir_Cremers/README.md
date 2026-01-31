# Méthodologie

## Objectifs
•	comprendre comment fonctionne un EDR

•	analyser la différence entre détection statique et détection comportementale

•	replacer la technique d’obfuscation utilisée dans l’exercice 3 dans un contexte de défense plus pointu (EDR)
 

## Mise en place
La mise en place des objectifs de l'exercice tels que définis en supra suppose la planification suivante : 

### Conserver le code de l’exercice 3 (injecteur + shellcode obfusqué)
•	Un injecteur écrit en C# comportant un shellcode simple (MessageBox) obfusqué via XOR dont le déchiffrement se fait au runtime, juste avant l’exécution en mémoire. Ce qui permet d’éviter la détection statique basée sur signatures et de pas stocker le shellcode en clair sur disque.

• Un programme simple nommé TargetDummy.exe, utilisé comme processus légitime cible de l’injection. Ceci permet de reproduire un scénario d’injection de code dans un processus existant, tout en restant dans un cadre totalement inoffensif.

### Renforcer l’environnement de test côté défense



### Observer le comportement du programme face à une défense de style EDR

### Analyser les résultats obtenus


## Retour d'expérience


