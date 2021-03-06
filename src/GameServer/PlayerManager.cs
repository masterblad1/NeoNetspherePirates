﻿using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace NeoNetsphere
{
    internal class PlayerManager : IReadOnlyCollection<Player>
    {
        private readonly IDictionary<ulong, Player> _players = new ConcurrentDictionary<ulong, Player>();

        public Player this[ulong id] => Get(id);

        public int Count => _players.Count;

        public IEnumerator<Player> GetEnumerator()
        {
            return _players.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Player Get(ulong id)
        {
            _players.TryGetValue(id, out var plr);
            return plr;
        }

        public Player Get(string nick)
        {
            return _players.Values.FirstOrDefault(plr =>
                plr.Account.Nickname != null &&
                plr.Account.Nickname.Equals(nick, StringComparison.InvariantCultureIgnoreCase));
        }

        public void Add(Player plr)
        {
            if (!_players.TryAdd(plr.Account.Id, plr))
                throw new Exception("Player " + plr.Account.Id + " already exists");
        }

        public void Remove(Player plr)
        {
            Remove(plr.Account.Id);
        }

        public void Remove(ulong id)
        {
            _players.Remove(id);
        }

        public bool Contains(Player plr)
        {
            return Contains(plr.Account.Id);
        }

        public bool Contains(ulong id)
        {
            return _players.ContainsKey(id);
        }
    }
}
