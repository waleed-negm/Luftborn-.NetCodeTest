import {Item} from '../../items/interfaces/IItem';

export interface Categry {
	id: string;
	name: string;
	items: Item[];
}
