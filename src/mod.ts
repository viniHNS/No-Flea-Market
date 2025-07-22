/* eslint-disable @typescript-eslint/naming-convention */

import { DependencyContainer } from "tsyringe";

import { IPostDBLoadMod } from "@spt/models/external/IPostDBLoadMod";
import { DatabaseServer } from "@spt/servers/DatabaseServer";
import { ILogger } from "@spt/models/spt/utils/ILogger";
import { LogTextColor } from "@spt/models/spt/logging/LogTextColor";
import { IPreSptLoadMod } from "@spt/models/external/IPreSptLoadMod";

class Mod implements IPostDBLoadMod, IPreSptLoadMod {
    private readonly MOD_NAME = "[ViniHNS] NUTS (No Unfair Trade System)";

    public preSptLoad(container: DependencyContainer): void {
        const logger = container.resolve<ILogger>("WinstonLogger");
        logger.logWithColor(`${this.MOD_NAME}: Loading...`, LogTextColor.GREEN);
    }

    public postDBLoad(container: DependencyContainer): void {
        const logger = container.resolve<ILogger>("WinstonLogger");
        
        try {
            const databaseServer = container.resolve<DatabaseServer>("DatabaseServer");
            
            // Get a reference to the database tables
            const tables = databaseServer.getTables();
     
            // Set flea market minimum level to 999 (effectively disabling it)
            tables.globals.config.RagFair.minUserLevel = 999;

        } catch (error) {
            logger.error(`${this.MOD_NAME}: Error during loading: ${error}`);
        }
    }
}

module.exports = { mod: new Mod() };