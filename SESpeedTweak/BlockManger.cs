using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SESpeedTweak
{
    class BlockManger
    {
        private XDocument xdoc;
        public List<Block> blocks { get; set; }        
        private string filename;

        public BlockManger(string filepath)
        {
            filename = filepath;
            loadData();
        }

        private void loadData()
        {
            xdoc = XDocument.Load(filename);

            var allblocks = from b in xdoc.Descendants("Definition")
                            select new Block(b);

            blocks = allblocks.OrderBy(b => b.Name).ToList();
        }

        public void increaseSpeedByMultiplier(decimal multiplier)
        {
            foreach(Block b in blocks)
            {
                b.BuildTime = b.BuildTime / Math.Abs(multiplier);
            }
        }

        public void decreaseSpeedByMultiplier(decimal multiplier)
        {
            foreach (Block b in blocks)
            {
                b.BuildTime = b.BuildTime * Math.Abs(multiplier);
            }
        }

        public void resetBlocks()
        {
            xdoc = null;
            blocks = null;

            loadData();            
        }

        public void Save()
        {
            xdoc.Save(filename);
        }

    }
}
