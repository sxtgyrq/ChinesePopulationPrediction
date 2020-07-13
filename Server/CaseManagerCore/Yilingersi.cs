using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaseManagerCore
{
    public class Yilingersi : CaseManager
    {
        //"{"grid":{"size":4,"cells":[[null,null,null,null],[null,null,null,null],[null,{"position":{"x":2,"y":1},"value":2},null,null],[null,{"position":{"x":3,"y":1},"value":2},null,null]]},"score":0,"over":false,"won":false,"keepPlaying":false}"

        item[,] grid;
        int size = 4;
        //private bool cellsAvailable
        //{
        //    get
        //    {
        //        for (int i = 0; i < 4; i++)
        //        {
        //            for (int j = 0; j < 4; j++)
        //            {
        //                if (this.grid[i, j] == null)
        //                {
        //                    return true;
        //                }
        //            }
        //        }
        //        return false;
        //    }
        //}

        public Yilingersi()
        {
            this.rm = new Random(DateTime.Now.GetHashCode());
            this.grid = new item[4, 4]
            {
{ null, null, null, null },
{ null, null, null, null },
{ null, null, null, null },
{ null, null, null, null },
            };


            List<item> addObjs = new List<item>();
            for (int i = 0; i < 2; i++)
            {
                addObjs.Add(this.addRandomTile());
            }
            PassObj p = new PassObj()
            {
                ObjType = "setrm2",
                msg = Newtonsoft.Json.JsonConvert.SerializeObject(addObjs),
                showContinue = true,
                showIsError = false,
                isEnd = false,
                ObjID = $"{idPrevious}{startId++}",
                styleStr = "msg"

            };
            this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
            Console.WriteLine(this.msg);
            this.step = 0;
            //PassObj p = new PassObj()
            //{
            //    ObjType = "setrm2",
            //    msg = $"老师您好，现在是{DateTime.Now.ToString("yyyy年MM月dd日 HH:mm")}。我给您介绍一下本软件。",
            //    showContinue = true,
            //    showIsError = false,
            //    isEnd = false,
            //    ObjID = $"{idPrevious}{startId++}",
            //    styleStr = "msg"

            //};
            //this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
        }

        class item
        {
            internal object mergedFrom;

            public position position { get; set; }
            public int value { get; set; }

            public position previousPosition { get; set; }
            internal void savePosition()
            {
                this.previousPosition = new position()
                {
                    x = this.position.x,
                    y = this.position.y
                };
            }

            internal void updatePosition(position cell)
            {
                this.position.x = cell.x;
                this.position.y = cell.y;
            }
        }
        class position
        {
            public int x { get; set; }
            public int y { get; set; }
        }
        item addRandomTile()
        {
            if (cellsAvailable())
            {
                var value = this.rm.Next(100) < 90 ? 1 : 2;
                var p = randomAvailableCell();
                this.grid[p[0], p[1]] =
                    new item()
                    {
                        position = new position()
                        {
                            x = p[0],
                            y = p[1]
                        },
                        value = value
                    };
                return this.grid[p[0], p[1]];
            }
            else
            {
                return null;
            }
        }

        position[] availableCells()
        {
            var cells = new List<position>();
            for (int i = 0; i < this.size; i++)
            {
                for (int j = 0; j < this.size; j++)
                {
                    if (this.grid[i, j] == null)
                        cells.Add(new position()
                        {
                            x = i,
                            y = j
                        });
                }
            }
            return cells.ToArray();
        }

        private bool cellsAvailable()
        {
            if (availableCells().Length == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //private bool cellsAvailable()
        //{
        //    return true;
        //}

        int[] randomAvailableCell()
        {

            List<int[]> indexs = new List<int[]>();

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (this.grid[i, j] == null)
                    {
                        indexs.Add(new int[] { i, j });
                    }
                }
            }
            if (indexs.Count > 0)
            {
                return indexs[this.rm.Next(indexs.Count)];
            }
            else
            {
                throw new Exception("randomAvailableCell");
            }

            //var cells = this.availableCells();

            //if (cells.length)
            //{
            //    return cells[Math.floor(Math.random() * cells.length)];
            //}
        }
        //class addRandomTile ()
        //{
        //    if (this.grid.cellsAvailable())
        //    {
        //        var value = Math.random() < 0.9 ? 2 : 4;
        //        var tile = new Tile(this.grid.randomAvailableCell(), value);

        //        this.grid.insertTile(tile);
        //    }
        //};

        public new void errorRecovery()
        {
            this.step++;

            PassObj p = new PassObj()
            {
                ObjType = "next",

            };
            this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
            Console.WriteLine("调用了errorRecovery");
        }

        public void Continue()
        {
            if (gameIsOver())
            {

            }
            else
            {
                var direction = this.step % 4;

            }

        }

        private bool gameIsOver()
        {
            return false;
        }
        position getVector(int direction)
        {
            switch (direction)
            {
                case 0:
                    return new position
                    {
                        x = 0,
                        y = -1
                    };
                case 1:
                    return new position
                    {
                        x = 1,
                        y = 0
                    };
                case 2:
                    return new position
                    {
                        x = 0,
                        y = 1
                    };
                case 3:
                    return new position
                    {
                        x = -1,
                        y = 0
                    };
                default:
                    {
                        throw new Exception($"{direction}");
                    }
            }
        }
        bool hasReached1024 = false;
        public void move(int v)
        {
            if (hasReached1024)
            {
                InputAddress();
            }
            else
            {
                var vector = this.getVector(v);
                var traversals = this.buildTraversals(vector);
                var moved = false;
                this.prepareTiles();

                for (var i = 0; i < traversals.x.Length; i++)
                {
                    for (var j = 0; j < traversals.y.Length; j++)
                    {
                        var cell = new position()
                        {
                            x = traversals.x[i],
                            y = traversals.y[j],
                        };
                        var tile = this.cellContent(cell);
                        if (tile != null)
                        {
                            var positions = this.findFarthestPosition(cell, vector);
                            var next = cellContent(positions.next);

                            if ((next != null) && (next.value == tile.value) && (next.mergedFrom == null))
                            {
                                var merged = new item()
                                {
                                    position = positions.next,
                                    value = tile.value * 2
                                };
                                merged.mergedFrom = new item[] { tile, next };
                                this.insertTile(merged);
                                this.removeTile(tile);

                                this.score += merged.value;

                                if (merged.value == 1024)
                                {
                                    this.hasReached1024 = true;
                                }
                                //this.
                                //(positions.next, tile.value * 2);
                            }
                            else
                            {
                                this.moveTile(tile, positions.farthest);
                            }
                            if (!positionsEqual(cell, tile))
                            {
                                moved = true;
                            }
                        }
                    }
                }

                if (moved)
                {
                    this.addRandomTile();
                }

                List<string> result = new List<string>();
                PassObj p = new PassObj()
                {
                    ObjType = "moveClick",
                    msg = Newtonsoft.Json.JsonConvert.SerializeObject(new { moved = moved, grid = this.grid }),
                    showContinue = true,
                    showIsError = false,
                    isEnd = false,
                    ObjID = $"{idPrevious}{startId++}",
                    styleStr = "msg"

                };
                this.msg = Newtonsoft.Json.JsonConvert.SerializeObject(p);
            }
        }

        private bool positionsEqual(position first, item second)
        {
            return first.x == second.position.x && first.y == second.position.y;
        }

        private void removeTile(item tile)
        {
            this.grid[tile.position.x, tile.position.y] = null;
        }

        private void insertTile(item tile)
        {
            this.grid[tile.position.x, tile.position.y] = tile;
        }

        private void moveTile(item tile, position cell)
        {
            this.grid[tile.position.x, tile.position.y] = null;
            this.grid[cell.x, cell.y] = tile;
            tile.updatePosition(cell);
        }

        class FarthestPosition
        {
            public position farthest { get; set; }
            public position next { get; set; }
        }
        private FarthestPosition findFarthestPosition(position cell, position vector)
        {
            position previous;
            do
            {
                previous = cell;
                cell = new position()
                {
                    x = previous.x + vector.x,
                    y = previous.y + vector.y
                };

            } while (this.withinBounds(cell) && cellsAvailable(cell));

            return new FarthestPosition()
            {
                farthest = previous,
                next = cell
            };

            //        var previous;

            //        // Progress towards the vector direction until an obstacle is found
            //        do
            //        {
            //            previous = cell;
            //            cell = { x: previous.x + vector.x, y: previous.y + vector.y };
            //        } while (this.grid.withinBounds(cell) &&
            //                 this.grid.cellAvailable(cell));

            //        return {
            //            farthest: previous,
            //next: cell // Used to check if a merge is required
        }

        private bool cellsAvailable(position cell)
        {
            return !this.cellOccupied(cell);
        }

        private bool cellOccupied(position cell)
        {
            var x = cellContent(cell);
            if (x == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool withinBounds(position cell)
        {
            return cell.x >= 0 && cell.x < this.size &&
        cell.y >= 0 && cell.y < this.size;
        }


        private item cellContent(position cell)
        {
            if (cell.x >= 0 && cell.x < this.size && cell.y >= 0 && cell.y < this.size)
            {
                return this.grid[cell.x, cell.y];
            }
            else
            {
                return null;
            }
        }

        private void prepareTiles()
        {
            for (var i = 0; i < this.size; i++)
            {
                for (var j = 0; j < this.size; j++)
                {
                    if (this.grid[i, j] == null) { }
                    else
                    {
                        this.grid[i, j].mergedFrom = null;
                        this.grid[i, j].savePosition();
                    }
                }
            }
        }

        class Traversals
        {
            public int[] x { get; set; }
            public int[] y { get; set; }
        }

        private Traversals buildTraversals(position vector)
        {
            var traversals = new Traversals();

            List<int> a, b;
            a = new List<int>();
            b = new List<int>();
            for (var pos = 0; pos < this.size; pos++)
            {
                a.Add(pos);
                b.Add(pos);
            }
            traversals.x = a.ToArray();
            traversals.y = b.ToArray();

            // Always traverse from the farthest cell in the chosen direction
            if (vector.x == 1) traversals.x = traversals.x.Reverse().ToArray();
            if (vector.y == 1) traversals.y = traversals.y.Reverse().ToArray();

            return traversals;
        }

        public string SaveAddress(string address)
        {
            MysqlCore.BaseItem b = new MysqlCore.BaseItem("yilingersi");
            //b.AddAddressValue(address, out moneycountAddV);
            return this.SaveAddress(address, b.xunzhangName, 0, b.AddAddressValue);
        }
    }
}
