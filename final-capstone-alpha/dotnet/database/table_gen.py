#! /bin/bash/python3


Y = [0,  'Y']
F = [1,  'F']
H = [2,  'H']
L = [3,  'L']
W = [4,  'W']
I = [5,  'I']
M = [6,  'M']
Q = [7,  'Q']
C = [8,  'C']
V = [9,  'V']
T = [10, 'T']
D = [11, 'D']
E = [12, 'E']
S = [13, 'S']
R = [14, 'R']
K = [15, 'K']
A = [16, 'A']
N = [17, 'N']
G = [18, 'G']
P = [19, 'P']

# 20x20 grid
Y_AXIS = [Y, F, H, L, W, I, M, Q, C, V, T, D, E, S, R, K, A, N, G, P]
X_AXIS = [Y, F, H, L, W, I, M, Q, C, V, T, D, E, S, R, K, A, N, G, P]


class Cell:
    def __init__(self, x, y, color='', acid=''):
        self.acid = acid
        self.color = color
        self.x = x
        self.y = y


def generate_table():
    table = []
    for i in range(20):
        row = []
        for j in range(20):
            row.append(Cell(X_AXIS[i], Y_AXIS[j]))
        table.append(row)
    return table


cell_grid = generate_table()

def set_by_letters(letter1, letter2, color, acid):
    for row in cell_grid:
        for cell in row:
            if cell.x[1] == letter1 and cell.y[1] == letter2:
                cell.color = color
                cell.acid = acid


def set_cells():
    # F on x axis
    set_by_letters('F', 'Y', 'blue', 'X')
    set_by_letters('F', 'F', 'blue', 'X')
    set_by_letters('F', 'H', 'blue', 'X')
    set_by_letters('F', 'L', 'green', 'X')
    set_by_letters('F', 'W', 'green', 'X')
    set_by_letters('F', 'I', 'green', 'X')
    set_by_letters('F', 'M', 'green', 'X')
    set_by_letters('F', 'Q', 'green', 'X')
    set_by_letters('F', 'C', 'green', 'X')
    set_by_letters('F', 'V', 'green', 'X')
    set_by_letters('F', 'T', 'green', 'X')
    set_by_letters('F', 'D', 'green', 'X')
    set_by_letters('F', 'E', 'green', 'X')
    set_by_letters('F', 'S', 'green', 'X')
    set_by_letters('F', 'R', 'yellow', 'X')
    set_by_letters('F', 'K', 'yellow', 'X')
    set_by_letters('F', 'A', 'yellow', 'X')
    set_by_letters('F', 'N', 'yellow', 'X')
    set_by_letters('F', 'G', 'red', 'X')
    set_by_letters('F', 'P', 'red', 'X')

    # H on x axis
    set_by_letters('H', 'P', 'red', 'J')

    # I on x axis
    set_by_letters('I', 'M', 'green', 'B')

    # M on x axis
    set_by_letters('M', 'F', 'green', 'X')
    set_by_letters('M', 'M', 'green', 'X')
    set_by_letters('M', 'E', 'yellow', 'X')
    set_by_letters('M', 'K', 'yellow', 'X')
    set_by_letters('M', 'A', 'yellow', 'X')
    set_by_letters('M', 'P', 'red', 'X')

    # C on x axis
    set_by_letters('C', 'M', 'green', 'C')

    # V on x axis
    set_by_letters('V', 'M', 'yellow', 'H')

    # T on x axis
    set_by_letters('T', 'F', 'green', 'A')
    set_by_letters('T', 'H', 'green', 'K')

    set_by_letters('T', 'M', 'yellow', 'A')

    # D on x axis
    set_by_letters('D', 'M', 'yellow', 'D')

    # E on x axis
    set_by_letters('E', 'F', 'green', 'X')

    set_by_letters('E', 'M', 'yellow', 'N')
    set_by_letters('E', 'Q', 'yellow', 'C')

    set_by_letters('E', 'D', 'red', 'X')
    set_by_letters('E', 'K', 'red', 'X')
    set_by_letters('E', 'A', 'red', 'X')
    set_by_letters('E', 'P', 'red', 'X')

    # S on x axis
    set_by_letters('S', 'M', 'yellow', 'L')

    # K on x axis
    set_by_letters('K', 'F', 'green', 'X')
    set_by_letters('K', 'M', 'yellow', 'X')

    set_by_letters('K', 'D', 'red', 'X')
    set_by_letters('K', 'K', 'red', 'X')
    set_by_letters('K', 'A', 'red', 'X')
    set_by_letters('K', 'P', 'red', 'X')

    # A on x axis
    set_by_letters('A', 'F', 'yellow', 'X')
    set_by_letters('A', 'M', 'yellow', 'X')

    set_by_letters('A', 'D', 'red', 'X')
    set_by_letters('A', 'K', 'red', 'X')
    set_by_letters('A', 'A', 'red', 'X')
    set_by_letters('A', 'P', 'red', 'X')

    # G on x axis
    set_by_letters('G', 'M', 'yellow', 'F')
    set_by_letters('G', 'A', 'red', 'I')

    # P on x axis
    set_by_letters('P', 'F', 'yellow', 'X')
    set_by_letters('P', 'M', 'yellow', 'X')

    set_by_letters('P', 'D', 'red', 'X')
    set_by_letters('P', 'K', 'red', 'X')
    set_by_letters('P', 'A', 'red', 'G')
    set_by_letters('P', 'P', 'red', 'X')


def cell_to_str(cell):
    #x
    x = str(cell.x[0])
    y = str(cell.y[0])
    letter1 = cell.x[1]
    letter2 = cell.y[1]
    color = cell.color
    acid = cell.acid
    return x + ',' + y + ' | ' + letter1 + letter2 + ' | ' + color + ' ' + acid

def cell_as_insert(cell):
    x = str(cell.x[0])
    y = str(cell.y[0])
    letter1 = cell.x[1]
    letter2 = cell.y[1]
    color = cell.color
    acid = cell.acid
    return 'INSERT INTO cells (x_cord, y_cord, letter_x, letter_y, color, acid) VALUES (' + x + ',' + y + ',\'' + letter1 + '\',\'' + letter2 + '\',\'' + color + '\',\'' + acid + '\');'

def print_sql(cell):
    print(cell_as_insert(cell))



def write_to_file():
    file = open('cells.sql', 'w')

    header ="""
-- AUTO GENERATED FILE DO NOT EDIT!!!!\n
USE final_capstone
GO
    """
    file.write(header + '\n\n\n')

    for row in cell_grid:
        for cell in row:
            file.write(cell_as_insert(cell) + '\n')
    file.close()

def main():
    set_cells()
    write_to_file()



main()
