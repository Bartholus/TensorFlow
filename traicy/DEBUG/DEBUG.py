import pstats
p = pstats.Stats('FilterResult')

# p.strip_dirs().sort_stats(-1).print_stats()
#
p.sort_stats("tottime")
p.print_stats()


#$ python -m cProfile -o profiling_results profile_test.py
