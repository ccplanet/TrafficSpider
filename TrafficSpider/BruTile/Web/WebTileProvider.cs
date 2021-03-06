﻿// Copyright (c) BruTile developers team. All rights reserved. See License.txt in the project root for license information.

using BruTile.Cache;
using System;

namespace BruTile.Web
{
    public class WebTileProvider : ITileProvider
    {
        private readonly IPersistentCache<byte[]> _persistentCache;
        public IRequest Request { get; set; }
        private readonly Func<Uri, byte[]> _fetchTile;

        public WebTileProvider(IRequest request = null, IPersistentCache<byte[]> persistentCache = null,
            Func<Uri, byte[]> fetchTile = null)
        {
            Request = request ?? new NullRequest();
            _persistentCache = persistentCache ?? new NullCache();
            _fetchTile = fetchTile ?? (RequestHelper.FetchImage);
        }

        public byte[] GetTile(TileInfo tileInfo)
        {
            var bytes = _persistentCache.Find(tileInfo.Index);
            if (bytes == null)
            {
                bytes = _fetchTile(Request.GetUri(tileInfo));
                if (bytes != null) _persistentCache.Add(tileInfo.Index, bytes);
            }
            return bytes;
        }
    }
}
