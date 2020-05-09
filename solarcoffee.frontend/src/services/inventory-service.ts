import axios from 'axios';
import { IProductInventory } from '@/types/Product';
import { IShipment } from '@/types/Shipment';
import { IInventoryTimeLine } from '@/types/InventoryGraph';

export class InventoryService {
    API_URL = process.env.VUE_APP_API_URL;

    public async getInventory(): Promise<IProductInventory[]> {
        try {
            const result = await axios.get(`${this.API_URL}/inventory`);
            return result.data;
        } catch (error) {
            console.error('Error', error);
            return error;
        }
    }

    public async updateInventoryQuantity(shipment: IShipment) {
        try {
            const result = await axios.patch(`${this.API_URL}/inventory`, shipment);
            return result.data;
        } catch (error) {
            console.error('Error', error);
            return error;
        }
    }

    public async getSnapShotHistory(): Promise<IInventoryTimeLine> {
        const result = await axios.get(`${this.API_URL}/inventory/snapshot`);
        return result.data;
    }
}