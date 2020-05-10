export interface IInventoryTimeLine {
    timeLine: Date[];
    productInventorySnapshots: IInventorySnapshot[]
}

export interface IInventorySnapshot {
    productId: number;
    quantityOnHand: number[];
}