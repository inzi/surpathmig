using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inzibackend.Migrations.Seed.Surpath
{
    internal class DrugSeedDataClass
    {
        public string test_category_name;
        public string internal_name;
        public int DNCDNID;
        public int DNCTCID;
        public string test_panel_name;
        public string test_panel_description;
        public int TPCATID;
        public double? test_panel_cost;
        public int tpdn_test_panel_id;
        public int tpdn_drug_name_id;
        public string drug_name;
        public string drug_code;
        public double? ua_screen_value;
        public double? ua_confirmation_value;
        public string ua_unit_of_measurement;
        public double? hair_screen_value;
        public double? hair_confirmation_value;
        public string hair_unit_of_measurement;
    }
}
