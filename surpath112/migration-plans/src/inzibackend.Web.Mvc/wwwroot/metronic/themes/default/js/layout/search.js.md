# Modified
## Filename
search.js
## Relative Path
inzibackend.Web.Mvc\wwwroot\metronic\themes\default\js\layout\search.js
## Language
JavaScript
## Summary
The modified file introduces changes to the KTLayoutSearch class by swapping the implementation logic of two functions (processs and clear) which control the visibility of elements based on a random number. Additionally, an event listener for the advanced options form search element is added.
## Changes
1. In processs function: Replaced 'add' with 'remove' in mainElement.classList operation.
2. In clear function: Replaced 'remove' with 'add' in resultsElement.classList operation.
3. Added an event listener for advancedOptionsFormSearchElement in handleAdvancedOptionsForm.
## Purpose
Enhance search functionality by implementing more sophisticated visibility control and additional interactive features.
