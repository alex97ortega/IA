// PruebaAlgoritmoPuzzle8.cpp: define el punto de entrada de la aplicación de consola.
//

#include <vector>
#include <iostream>
#include "stdafx.h"


using namespace std;
struct Position { int x; int y; };

class Board
{

public:
	vector<vector<int>> board;

	Board() :board(0)
	{
		board = vector<vector<int>>();
		board.resize(3);
		for (size_t i = 0; i < 3; i++)
		{
			for (size_t j = 0; j < 3; j++)
			{
				board[i].push_back(i + j);
			}
		}
	};
	~Board() {};

	void mezcla() 
	{
		bool used[9]; 
		for (int i = 0; i < 9; i++) { used[i] = false; };
		
		bool added = false;
		int r;
		for (int i = 0; i < 3; i++) 
		{
			for (size_t j = 0; j < 3; j++)
			{
				while (!added)
				{
					r = rand() % 9;
					if (!used[r]) 
					{
						board[i][j] = r;
						added = true;
					}
				}
				added = false;
			}
		}
	};

	void print() 
	{
		for (size_t i = 0; i < 3; i++)
		{
			for (size_t j = 0; j < 3; j++)
			{
				cout << board[i][j] << " ";
			}
			cout << endl;
		}
	};

};


int main()
{
	Board board = Board();
	board.print();
	cin.get();
    return 0;
}

