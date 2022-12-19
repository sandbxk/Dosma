import { Status } from './StatusEnum';
import { CategoryEnum } from './CategoryEnum';

export interface Item {
  id: number;
  title: string;
  quantity: number;
  groceryListId: number;
  status: Status;
  category: string;
  index: number;
}

export class ItemDTO {
  id: number = 0;
  title: string = '';
  quantity: number = 1;
  groceryListId: number = 0;
  status: Status = Status.Unchecked;
  category: string = 'None';
}

export function dtoToItem(dto: ItemDTO): Item {
  return {
    id: dto.id,
    title: dto.title,
    quantity: dto.quantity,
    groceryListId: dto.groceryListId,
    status: dto.status,
    category: dto.category,
    index: 0,
  };
}

export function itemToDto(item: Item): ItemDTO {
  return {
    id: item.id,
    title: item.title,
    quantity: item.quantity,
    groceryListId: item.groceryListId,
    status: item.status,
    category: item.category,
  };
}
