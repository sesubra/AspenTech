# AspenTech
Project solutions for problems given by AspenTech. Submitted by Senthil Subramanian

# Assumptions

##Grams Converter

#####Assumptions
List of assumptions made in this project

* Periodic table details will be read from a file named `periodicTable.json` from the same folder as the executable
* User's input will be read from a file named `userInput.json` from the same folder as the executable
* No UI is needed or this program

#####Program Operation
* Program will calculate the total weight of the components specified in the `userInput.json` file and will terminate. Single use.
* Program will output the details of the `userInput.json` before outputting the result
* Program will run only in Console
* Compatible only with Windows OS

#####Recommendation
* Use console window with Black background


##Nodes

#####Assumptions
* Initial formation of node structure will be read from file named `nodestructure.json` from the same folder as the executable
* On successful loading of the initial node structure, program will validate initial connection of all nodes to root and shows the disconnected nodes
* Existing edge can be removed one-by-one
* Edges cannot be added back to the structure, even the removed ones
* Program does not ignore the disonnected nodes in the further steps

#####Removing an Edge
* User should first select the FROM (starting) node to list all the edges going out of the selected node
* User should select the available nodes to remove the edge

#####Program Operation
* Once the user removes an edge, the program will show all the disconnected edge
* Program terminates when the user inputs a non-existing node or empty
* When a user removes an additional edge, program includes all the disconnected nodes (including the disconnected ones from removing edges from the previous steps)

#####Recommendation
* Use console window with Black background
