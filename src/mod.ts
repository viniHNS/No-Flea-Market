import { DependencyContainer } from "tsyringe";

import { IPostDBLoadMod } from "@spt/models/external/IPostDBLoadMod";
import { DatabaseServer } from "@spt/servers/DatabaseServer";
import { ILogger } from "@spt/models/spt/utils/ILogger";
import { LogTextColor } from "@spt/models/spt/logging/LogTextColor";

import { Money } from "@spt/models/enums/Money";
import { Traders } from "@spt/models/enums/Traders";

import { FluentAssortConstructor as FluentAssortCreator } from "./fluentTraderAssortCreator";
import { ItemTpl } from "@spt/models/enums/ItemTpl";
import { HashUtil } from "@spt/utils/HashUtil";
import { IPreSptLoadMod } from "@spt/models/external/IPreSptLoadMod";

class Mod implements IPostDBLoadMod, IPreSptLoadMod
{
    private mod: string;
    private logger: ILogger;
    private fluentAssortCreator: FluentAssortCreator;

    constructor() {
        this.mod = "NUTS (No Unfair Trade System)"; // Set name of mod so we can log it to console later
    }

    public preSptLoad(container: DependencyContainer): void 
    {
         
        // Get SPT code/data we need later
        const hashUtil: HashUtil = container.resolve<HashUtil>("HashUtil");
        this.logger = container.resolve<ILogger>("WinstonLogger");
        this.fluentAssortCreator = new FluentAssortCreator(hashUtil, this.logger);
        
    }

    public postDBLoad(container: DependencyContainer): void
    {

        const databaseServer: DatabaseServer = container.resolve<DatabaseServer>("DatabaseServer");

        // Get a reference to the database tables
        const tables = databaseServer.getTables();


        // Get globals settings and set flea market min level to be 1
        tables.globals.config.RagFair.minUserLevel = 999;

        // Add 203 dorms key to therapist
        this.fluentAssortCreator
            .createSingleAssortItem(ItemTpl.KEY_DORM_ROOM_203)
            .addStackCount(13)
            .addBarterCost(ItemTpl.MEDKIT_SALEWA_FIRST_AID_KIT, 1)
            .addBarterCost(ItemTpl.DRINK_BOTTLE_OF_WATER_06L, 2)
            .addBarterCost(ItemTpl.BARTER_MEDICAL_BLOODSET, 1)
            .addLoyaltyLevel(1)
            .export(tables.traders[Traders.THERAPIST]);

        // Add 214 dorms key to therapist
        this.fluentAssortCreator
            .createSingleAssortItem(ItemTpl.KEY_DORM_ROOM_214)
            .addStackCount(10)
            .addBarterCost(ItemTpl.MEDKIT_AI2, 3)
            .addBarterCost(ItemTpl.BARTER_PILE_OF_MEDS, 2)
            .addBarterCost(ItemTpl.BARTER_STRIKE_CIGARETTES, 1)
            .addLoyaltyLevel(1)
            .export(tables.traders[Traders.THERAPIST]);

        // Add Iron Gete key (Streets) to Prapor
        this.fluentAssortCreator
            .createSingleAssortItem(ItemTpl.KEY_IRON_GATE)
            .addStackCount(5)
            .addBarterCost(ItemTpl.GRENADE_F1_HAND, 1)
            .addBarterCost(ItemTpl.BARTER_WEAPON_PARTS, 2)
            .addBarterCost(ItemTpl.BARTER_GUNPOWDER_KITE, 1)
            .addLoyaltyLevel(2)
            .export(tables.traders[Traders.PRAPOR]);

        // Add Car dealership director's office room key to Jaeger
        this.fluentAssortCreator
            .createSingleAssortItem(ItemTpl.KEY_CAR_DEALERSHIP_DIRECTORS_OFFICE_ROOM)
            .addStackCount(2)
            .addBarterCost(ItemTpl.BARTER_GOLDEN_NECK_CHAIN, 2)
            .addBarterCost(ItemTpl.BARTER_BROKEN_GPHONE_X_SMARTPHONE, 2)
            .addBarterCost(ItemTpl.BARTER_GRAPHICS_CARD, 1)
            .addLoyaltyLevel(4)
            .export(tables.traders[Traders.JAEGER]);

        // Add Health resort office key with a blue tape to Therapist
        this.fluentAssortCreator
            .createSingleAssortItem(ItemTpl.KEY_HEALTH_RESORT_OFFICE_KEY_WITH_A_BLUE_TAPE)
            .addStackCount(2)
            .addBarterCost(ItemTpl.BARTER_MEDICAL_BLOODSET, 4)
            .addBarterCost(ItemTpl.BARTER_BOTTLE_OF_OLOLO_MULTIVITAMINS, 2)
            .addBarterCost(ItemTpl.BARTER_PILE_OF_MEDS, 10)
            .addBarterCost(ItemTpl.BARTER_TOILET_PAPER, 3)
            .addLoyaltyLevel(2)
            .export(tables.traders[Traders.THERAPIST]);    

        // Add Factory emergency exit key to Jaeger
        this.fluentAssortCreator
            .createSingleAssortItem(ItemTpl.KEY_FACTORY_EMERGENCY_EXIT)
            .addStackCount(2)
            .addBarterCost(ItemTpl.FOOD_MRE_RATION_PACK, 2)
            .addBarterCost(ItemTpl.FOOD_PACK_OF_INSTANT_NOODLES, 3)
            .addBarterCost(ItemTpl.FOOD_ARMY_CRACKERS, 3)
            .addLoyaltyLevel(1)
            .export(tables.traders[Traders.JAEGER]);
        
        
        this.logger.logWithColor(`[ViniHNS] ${this.mod} - Database Loaded`, LogTextColor.GREEN);

    }
}

export const mod = new Mod();
