export default {
    routes: [
        {
            method: 'GET',
            path: '/items',
            handler: 'items.getAll',
            config: { policies: ['admin::isAuthenticatedAdmin'] },
        },
        {
            method: 'GET',
            path: '/items/:id',
            handler: 'items.getById',
            config: { policies: ['admin::isAuthenticatedAdmin'] },
        },
        {
            method: 'POST',
            path: '/items',
            handler: 'items.create',
            config: { policies: ['admin::isAuthenticatedAdmin'] },
        },
        {
            method: 'PUT',
            path: '/items/:id',
            handler: 'items.update',
            config: { policies: ['admin::isAuthenticatedAdmin'] },
        },
        {
            method: 'DELETE',
            path: '/items/:id',
            handler: 'items.delete',
            config: { policies: ['admin::isAuthenticatedAdmin'] },
        },
    ],
};
