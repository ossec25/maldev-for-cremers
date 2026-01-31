# Méthodologie

## Objectifs
•	comprendre comment fonctionne un EDR

•	analyser la différence entre détection statique et détection comportementale

•	replacer la technique d’obfuscation utilisée dans l’exercice 3 dans un contexte de défense plus pointu (EDR)
 

## Mise en place
La mise en place des objectifs de l'exercice tels que définis en supra suppose la planification suivante : 

### Conserver le code de l’exercice 3
•	Un injecteur écrit en C# comportant un shellcode simple (MessageBox) obfusqué via XOR dont le déchiffrement se fait au runtime, juste avant l’exécution en mémoire. Ce qui permet d’éviter la détection statique basée sur signatures et de pas stocker le shellcode en clair sur disque. Il est à noter que le binaire a été effacé du dossier des téléchargements (où il se trouvait depuis l'exercice 3) pour recommencer la manipulation du malware depuis l'état initial de téléchargement.  

• Un programme simple nommé TargetDummy.exe, utilisé comme processus légitime cible de l’injection. Ceci permet de reproduire un scénario d’injection de code dans un processus existant, tout en restant dans un cadre totalement inoffensif.

### Renforcer l’environnement de test côté défense avec un aapproche EDR
Plutôt que d’installer un EDR lourd, la défense a été renforcée à l’aide de Microsoft Defender, déjà présent sur la machine, en activant les paramètres de style EDR :  
•	Dans Windows Security, la protection fournie par le cloud, la soumission automatique d'échantillons, la protection en temps réel, la protection contre les applications potentiellement indésirables, la protection contre les falsifications (Tamper protection)   
•	Dans le contrôle des applications et du navigateur, et plus précisément les paramètres de protection basée sur la réputation, activation de la vérification des applications et des fichiers, de SmartScreen pour Microsoft Edge et du blocage des applications potentiellement indésirables
•	Activation/durcissement des règles d’Attack Surface Reduction (ASR) en mode audit (et non blocage). Ceci permet d’observer le comportement sans bloquer l’exécution et sans fausser les résultats par un blocage artificiel.

### Observer le comportement du programme face à cette défense étendue de style EDR
L'observation du comportement s'effectue d'une part à l'examen de l'historique de protection des paramètres de sécurité Windows et d'autre part dans l'observateur d'évènements, dans la zone opérationnelle des applications et service logs dédiés à Windows Defender (voir screenshots). 

### Analyser les résultats obtenus


## Retour d'expérience


