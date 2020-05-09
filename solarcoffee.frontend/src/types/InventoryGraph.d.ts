export interface IInventoryTimeLine {
    timeline: Date[];
    productInventorySnapshot: IInventorySnapshot[]
}

export interface IInventorySnapshot {
    productId: number;
    quantityOnHand: number[];
}