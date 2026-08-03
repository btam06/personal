import { Main } from "@strapi/design-system";
import { useIntl } from "react-intl";

import { getTranslation } from "../utils/getTranslation";

import { useState, useEffect } from 'react';
import { useFetchClient } from "@strapi/strapi/admin";

interface Item {
    id: string;
    name: string;
}

const List = () => {
    const [items, setItems]     = useState<Item[]>([]);
    const [name, setName]       = useState('');
    const [loading, setLoading] = useState(true);

    const { get, post, del } = useFetchClient();

    const loadItems = async () => {
      const { data } = await get('/items/list');
      setItems(Array.isArray(data) ? data : []);
      setLoading(false);
    };

    const handleCreate = async () => {
      await post('/items/view', { name });
      setName('');
      loadItems();
    };

    const handleDelete = async (id: string) => {
      await del(`/items/delete/${id}`);
      loadItems();
    };

    useEffect(() => {
        loadItems();
    }, []);

    if (loading) return <div>Loading...</div>;

    return (
    <div style={{ padding: '2rem' }}>
        <h1>Items</h1>

        <div style={{ marginBottom: '1rem' }}>
            <input value={name} onChange={(e) => setName(e.target.value)} placeholder="Item name" />
            <button onClick={handleCreate}>Create</button>
        </div>

        <ul>
            {items.map((item) => (
            <li key={item.id}>
                {item.name}
                <button onClick={() => handleDelete(item.id)}>Delete</button>
            </li>
            ))}
        </ul>
    </div>
    );
}

export { List };
